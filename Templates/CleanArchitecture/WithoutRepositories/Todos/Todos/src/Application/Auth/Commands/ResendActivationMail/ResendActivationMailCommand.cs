namespace Todos.Application.Auth.Commands.ResendActivationMail;

public record ResendActivationMailCommand : IRequest
{
    public required string Username { get; init; }
}
