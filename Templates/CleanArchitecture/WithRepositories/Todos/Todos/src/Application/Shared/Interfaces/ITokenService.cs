using System.Security.Claims;

namespace Todos.Application.Shared.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(IEnumerable<Claim> claims);

    Task<string> GenerateRefreshTokenAsync(string userIdentifier);

    Task<ApplicationResult> ValidateRefreshTokenAsync(string refreshToken);

    Task<IEnumerable<Claim>> ReadTokenClaimsAsync(string token);
}
