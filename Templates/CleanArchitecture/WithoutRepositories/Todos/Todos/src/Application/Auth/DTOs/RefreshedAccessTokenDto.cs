namespace Todos.Application.Auth.DTOs;

public record RefreshedAccessTokenDto
{
    public required string AccessToken { get; init; }
}
