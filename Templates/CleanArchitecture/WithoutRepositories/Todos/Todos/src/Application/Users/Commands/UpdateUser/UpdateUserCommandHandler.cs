using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    ICurrentUser currentUser
) : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<UserDto> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.Id != _currentUser.Id)
        {
            throw new ForbiddenAccessException();
        }

        ApplicationResult<UserDto> result = await _applicationUserManager.UpdateUserAsync(command);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }

        UserDto user = Guard.Against.Null(result.Value);

        return user;
    }
}
