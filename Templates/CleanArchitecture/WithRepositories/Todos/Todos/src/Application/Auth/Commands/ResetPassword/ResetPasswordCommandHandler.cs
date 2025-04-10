namespace Todos.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthService authService)
    : IRequestHandler<ResetPasswordCommand>
{
    private readonly IAuthService _authService = authService;

    public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        ApplicationResult result = await _authService.ResetPasswordAsync(
            command.UserId,
            Uri.UnescapeDataString(command.ResetToken),
            command.NewPassword
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }
    }
}
