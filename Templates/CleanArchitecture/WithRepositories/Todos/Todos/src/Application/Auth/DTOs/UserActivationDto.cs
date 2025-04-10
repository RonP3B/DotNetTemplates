using Todos.Application.Users.DTOs;

namespace Todos.Application.Auth.DTOs;

public record UserActivationDto
{
    public required UserDto User { get; init; }

    public required string ActivationToken { get; init; }
}
