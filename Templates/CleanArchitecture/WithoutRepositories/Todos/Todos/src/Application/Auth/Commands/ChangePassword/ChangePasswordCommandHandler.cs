namespace Todos.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IAuthService authService, ICurrentUser currentUser)
    : IRequestHandler<ChangePasswordCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        if (_currentUser.Id != command.UserId)
        {
            throw new ForbiddenAccessException();
        }

        ApplicationResult result = await _authService.ChangePasswordAsync(
            command.UserId,
            command.CurrentPassword,
            command.NewPassword
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }
    }
}
