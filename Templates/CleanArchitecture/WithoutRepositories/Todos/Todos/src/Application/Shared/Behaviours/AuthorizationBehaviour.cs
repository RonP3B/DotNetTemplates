using System.Reflection;

namespace Todos.Application.Shared.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(
    ICurrentUser currentUser,
    IAuthService authService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IAuthService _authService = authService;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<AuthorizeAttribute> authorizeAttributes = request
            .GetType()
            .GetCustomAttributes<AuthorizeAttribute>();

        if (!authorizeAttributes.Any())
        {
            // Authorization is not required
            return await next();
        }

        if (_currentUser.Id == null)
        {
            throw new UnauthorizedAccessException();
        }

        await ValidateRolesAsync(_currentUser.Id, authorizeAttributes);

        await ValidatePoliciesAsync(_currentUser.Id, authorizeAttributes);

        // User is authorized
        return await next();
    }

    private async Task ValidateRolesAsync(
        string userId,
        IEnumerable<AuthorizeAttribute> authorizeAttributes
    )
    {
        List<string> roles = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .SelectMany(a => a.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Select(r => r.Trim())
            .Distinct()
            .ToList();

        if (roles.Count != 0)
        {
            foreach (string role in roles)
            {
                if (await _authService.HasRoleAsync(userId, role))
                {
                    return;
                }
            }

            throw new ForbiddenAccessException();
        }
    }

    private async Task ValidatePoliciesAsync(
        string userId,
        IEnumerable<AuthorizeAttribute> authorizeAttributes
    )
    {
        List<string> policies = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
            .Select(a => a.Policy)
            .Distinct()
            .ToList();

        foreach (var policy in policies)
        {
            if (!await _authService.HasPermissionAsync(userId, policy))
            {
                throw new ForbiddenAccessException();
            }
        }
    }
}
