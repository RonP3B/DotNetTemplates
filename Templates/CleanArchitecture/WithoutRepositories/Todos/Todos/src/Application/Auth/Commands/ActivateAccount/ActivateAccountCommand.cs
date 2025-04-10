namespace Todos.Application.Auth.Commands.ActivateAccount;

public record ActivateAccountCommand(string Username, string ActivationToken) : IRequest<string>;
