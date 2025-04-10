using System.Security.Claims;
using Todos.Application.Auth.DTOs;
using Todos.Application.Users.DTOs;

namespace Todos.Application.Auth.Queries.GetAccessToken;

public class GetAccessTokenQueryHandler(
    ITokenService tokenService,
    IApplicationUserManager applicationUserManager
) : IRequestHandler<GetAccessTokenQuery, RefreshedAccessTokenDto>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;

    public async Task<RefreshedAccessTokenDto> Handle(
        GetAccessTokenQuery query,
        CancellationToken cancellationToken
    )
    {
        ApplicationResult result = await _tokenService.ValidateRefreshTokenAsync(
            query.RefreshToken
        );

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(result.FlattenErrors());
        }

        IEnumerable<Claim> refreshTokenClaims = await _tokenService.ReadTokenClaimsAsync(
            query.RefreshToken
        );

        string userIdentifier = refreshTokenClaims
            .First(c => c.Type == ClaimTypes.NameIdentifier)
            .Value;

        UserDto user = await _applicationUserManager.GetUserByIdAsync(userIdentifier);

        List<Claim> accessTokenClaims =
        [
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            // More needed claims can be added here...
        ];

        return new RefreshedAccessTokenDto
        {
            AccessToken = await _tokenService.GenerateAccessTokenAsync(accessTokenClaims),
        };
    }
}
