namespace Todos.Application.Auth.DTOs;

public record AuthTokensDto
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }
}
