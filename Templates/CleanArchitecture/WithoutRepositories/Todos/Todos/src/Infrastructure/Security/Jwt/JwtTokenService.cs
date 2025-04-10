using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Todos.Application.Shared.Interfaces;
using Todos.Application.Shared.Models;

namespace Todos.Infrastructure.Security.Jwt;

public class JwtTokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public Task<string> GenerateAccessTokenAsync(IEnumerable<Claim> claims)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecretKey));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken accessToken =
            new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(accessToken));
    }

    public Task<string> GenerateRefreshTokenAsync(string userIdentifier)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecretKey));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [new(ClaimTypes.NameIdentifier, userIdentifier)];

        JwtSecurityToken refreshToken =
            new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                signingCredentials: creds,
                claims: claims
            );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(refreshToken));
    }

    public Task<ApplicationResult> ValidateRefreshTokenAsync(string refreshToken)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecretKey));

        TokenValidationParameters tokenValidationParameters =
            new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero,
            };

        JwtSecurityTokenHandler tokenHandler = new();

        try
        {
            ClaimsPrincipal validatedToken = tokenHandler.ValidateToken(
                refreshToken,
                tokenValidationParameters,
                out var tokenInfo
            );

            bool isValidRefreshToken = validatedToken != null && IsValidJwtSecurityToken(tokenInfo);

            if (isValidRefreshToken)
            {
                return Task.FromResult(ApplicationResult.Success());
            }
        }
        catch (SecurityTokenExpiredException)
        {
            return Task.FromResult(
                ApplicationResult.Failure("RefreshToken", "The refresh token has expired.")
            );
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return Task.FromResult(
                ApplicationResult.Failure("RefreshToken", "The refresh token signature is invalid.")
            );
        }
        catch (SecurityTokenValidationException ex)
        {
            return Task.FromResult(
                ApplicationResult.Failure("RefreshToken", $"Token validation failed: {ex.Message}")
            );
        }

        return Task.FromResult(ApplicationResult.Failure("RefreshToken", "Invalid refresh token."));
    }

    public Task<IEnumerable<Claim>> ReadTokenClaimsAsync(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        if (!tokenHandler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT token format.");
        }

        JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

        return Task.FromResult(jwtToken.Claims);
    }

    private static bool IsValidJwtSecurityToken(SecurityToken token)
    {
        if (token is not JwtSecurityToken jwtSecurityToken)
        {
            return false;
        }

        string algorithm = jwtSecurityToken.Header.Alg;

        bool isExpectedAlgorithm = algorithm.Equals(
            SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase
        );

        return isExpectedAlgorithm;
    }
}
