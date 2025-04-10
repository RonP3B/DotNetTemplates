using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Commands.PatchUser;

[Authorize]
public record PatchUserCommand : IRequest<UserDto>
{
    public required string Id { get; init; }

    public string? UserName { get; init; }

    public string? ImageKey { get; init; }
}
