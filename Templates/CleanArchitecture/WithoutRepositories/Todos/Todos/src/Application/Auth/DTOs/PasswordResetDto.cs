using Todos.Application.Users.DTOs;

namespace Todos.Application.Auth.DTOs;

public record PasswordResetDto
{
    public required string ResetToken { get; init; }

    public required UserDto User { get; init; }
}
