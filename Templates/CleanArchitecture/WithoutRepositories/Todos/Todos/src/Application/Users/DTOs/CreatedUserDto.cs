namespace Todos.Application.Users.DTOs;

public record CreatedUserDto
{
    public required UserDto User { get; init; }

    public required string ActivationToken { get; init; }
}
