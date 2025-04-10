namespace Todos.Application.Auth.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public required string ResetToken { get; init; }

    public required string UserId { get; init; }

    public required string NewPassword { get; init; }
}
