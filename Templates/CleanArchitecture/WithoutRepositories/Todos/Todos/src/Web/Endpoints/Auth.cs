using Microsoft.AspNetCore.Http.HttpResults;
using Todos.Application.Auth.Commands.ActivateAccount;
using Todos.Application.Auth.Commands.ChangePassword;
using Todos.Application.Auth.Commands.ForgotPassword;
using Todos.Application.Auth.Commands.Login;
using Todos.Application.Auth.Commands.ResendActivationMail;
using Todos.Application.Auth.Commands.ResetPassword;
using Todos.Application.Auth.DTOs;
using Todos.Application.Auth.Queries.GetAccessToken;

namespace Todos.Web.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(RefreshAccessToken, "access-token")
            .MapGet(RemoveRefreshToken, "logout")
            .MapGet(ActivateAccount, "account-activation")
            .MapPost(Login, "login")
            .MapPost(ChangePassword, "change-password")
            .MapPost(ResetPassword, "reset-password")
            .MapPost(ForgotPassword, "forgot-password")
            .MapPost(ResendAccountActivationMail, "resend-activation-mail");
    }

    public async Task<Ok<LoginResponseDto>> Login(
        HttpContext httpContext,
        IConfiguration configuration,
        ISender sender,
        LoginCommand command
    )
    {
        AuthTokensDto result = await sender.Send(command);

        int refreshTokenExpirationDays = configuration.GetValue<int>(
            "JwtSettings:RefreshTokenExpirationDays"
        );

        httpContext.Response.Cookies.Append(
            "refreshToken",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays),
            }
        );

        LoginResponseDto loginResponse = new() { AccessToken = result.AccessToken };

        return TypedResults.Ok(loginResponse);
    }

    public async Task<Ok<RefreshedAccessTokenDto>> RefreshAccessToken(
        ISender sender,
        HttpContext httpContext
    )
    {
        httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        RefreshedAccessTokenDto refreshedAccessToken = await sender.Send(
            new GetAccessTokenQuery(refreshToken ?? "")
        );

        return TypedResults.Ok(refreshedAccessToken);
    }

    public async Task<ContentHttpResult> ActivateAccount(
        ISender sender,
        string username,
        string token
    )
    {
        ActivateAccountCommand command = new(username, token);

        string result = await sender.Send(command);

        return TypedResults.Content(result, "text/html");
    }

    public async Task<NoContent> ResendAccountActivationMail(
        ISender sender,
        ResendActivationMailCommand command
    )
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<NoContent> ChangePassword(ISender sender, ChangePasswordCommand command)
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<NoContent> ForgotPassword(ISender sender, ForgotPasswordCommand command)
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<NoContent> ResetPassword(ISender sender, ResetPasswordCommand command)
    {
        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public NoContent RemoveRefreshToken(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions { HttpOnly = true, Secure = true }
        );

        return TypedResults.NoContent();
    }
}
