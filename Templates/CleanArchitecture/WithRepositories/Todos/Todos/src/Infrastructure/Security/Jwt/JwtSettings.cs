namespace Todos.Infrastructure.Security.Jwt;

public record JwtSettings
{
    public required string AccessTokenSecretKey { get; init; }

    public required string RefreshTokenSecretKey { get; init; }

    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public int AccessTokenExpirationMinutes { get; init; }

    public int RefreshTokenExpirationDays { get; init; }
}
