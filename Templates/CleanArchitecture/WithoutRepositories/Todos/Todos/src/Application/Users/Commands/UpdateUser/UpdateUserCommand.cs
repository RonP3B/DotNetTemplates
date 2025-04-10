using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Commands.UpdateUser;

[Authorize]
public record UpdateUserCommand : IRequest<UserDto>
{
    public required string Id { get; init; }

    public required string UserName { get; init; }

    public string? ImageKey { get; init; }
}
