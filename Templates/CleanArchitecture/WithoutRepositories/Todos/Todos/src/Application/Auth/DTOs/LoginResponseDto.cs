namespace Todos.Application.Auth.DTOs;

public record LoginResponseDto
{
    public required string AccessToken { get; init; }
}
