using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<UserDto>
{
    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public string? ImageKey { get; init; }
}
