using System.Security.Claims;
using Todos.Application.Auth.DTOs;
using Todos.Application.Users.DTOs;

namespace Todos.Application.Auth.Commands.Login;

public class LoginCommandHandler(ITokenService tokenService, IAuthService authService)
    : IRequestHandler<LoginCommand, AuthTokensDto>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAuthService _authService = authService;

    public async Task<AuthTokensDto> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        ApplicationResult<UserDto> result = await _authService.ValidateUserAsync(
            command.Username,
            command.Password
        );

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(result.FlattenErrors());
        }

        UserDto user = Guard.Against.Null(result.Value);

        List<Claim> accessTokenClaims =
        [
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            // More needed claims can be added here...
        ];

        string accessToken = await _tokenService.GenerateAccessTokenAsync(accessTokenClaims);

        string refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        return new AuthTokensDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }
}
