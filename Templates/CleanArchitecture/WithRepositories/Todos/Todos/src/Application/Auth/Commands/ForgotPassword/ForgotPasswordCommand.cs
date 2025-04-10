namespace Todos.Application.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand : IRequest
{
    public required string Username { get; init; }
}
