namespace Todos.Application.Users.DTOs;

public record UserDto
{
    public required string Id { get; init; }

    public required string UserName { get; init; }

    public required string Email { get; init; }

    public string? ImageKey { get; init; }
}
