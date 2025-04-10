using Todos.Application.Auth.TemplateModels;

namespace Todos.Application.Auth.Commands.ActivateAccount;

public class ActivateAccountCommandHandler(
    IAuthService authService,
    ITemplateService templateService
) : IRequestHandler<ActivateAccountCommand, string>
{
    private readonly IAuthService _authService = authService;
    private readonly ITemplateService _templateService = templateService;

    public async Task<string> Handle(
        ActivateAccountCommand command,
        CancellationToken cancellationToken
    )
    {
        ApplicationResult result = await _authService.ActivateAccountAsync(
            command.Username,
            Uri.UnescapeDataString(command.ActivationToken)
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }

        return await _templateService.GetRenderedTemplateAsync(
            new AccountActivatedTemplate(command.Username)
        );
    }
}
