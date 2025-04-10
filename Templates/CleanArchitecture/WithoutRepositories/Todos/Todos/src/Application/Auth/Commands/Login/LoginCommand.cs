using Todos.Application.Auth.DTOs;

namespace Todos.Application.Auth.Commands.Login;

public record LoginCommand : IRequest<AuthTokensDto>
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}
