using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Todos.Application.Auth.DTOs;
using Todos.Application.Shared.Interfaces;
using Todos.Application.Shared.Models;
using Todos.Application.Users.DTOs;

namespace Todos.Infrastructure.Identity;

public class IdentityAuthService(
    UserManager<ApplicationUser> userManager,
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService
) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory =
        userClaimsPrincipalFactory;

    public async Task<ApplicationResult<UserDto>> ValidateUserAsync(
        string userName,
        string password
    )
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return ApplicationResult<UserDto>.Failure("Credentials", "Invalid credentials.");
        }

        if (!user.EmailConfirmed)
        {
            return ApplicationResult<UserDto>.Failure(
                "Account",
                "Account not activated. Please verify your email address."
            );
        }

        return ApplicationResult<UserDto>.Success(user.Adapt<UserDto>());
    }

    public async Task<ApplicationResult> ActivateAccountAsync(
        string userName,
        string activationToken
    )
    {
        var user = await _userManager.FindByNameAsync(userName);

        Guard.Against.NotFound(userName, user);

        IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, activationToken);

        return identityResult.ToApplicationResult();
    }

    public async Task<PasswordResetDto> GetPasswordResetTokenAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        Guard.Against.NotFound(userName, user);

        return new PasswordResetDto
        {
            ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user),
            User = user.Adapt<UserDto>(),
        };
    }

    public async Task<ApplicationResult> ResetPasswordAsync(
        string userId,
        string resetToken,
        string newPassword
    )
    {
        var user = await _userManager.FindByIdAsync(userId);

        Guard.Against.NotFound(userId, user);

        IdentityResult identityResult = await _userManager.ResetPasswordAsync(
            user,
            resetToken,
            newPassword
        );

        return identityResult.ToApplicationResult();
    }

    public async Task<ApplicationResult> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword
    )
    {
        var user = await _userManager.FindByIdAsync(userId);

        Guard.Against.NotFound(userId, user);

        IdentityResult identityResult = await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword
        );

        return identityResult.ToApplicationResult();
    }

    public async Task<ApplicationResult<UserActivationDto>> GetActivationTokenAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        Guard.Against.NotFound(userName, user);

        if (user.EmailConfirmed)
        {
            return ApplicationResult<UserActivationDto>.Failure(
                "Activation",
                "Account is already activated."
            );
        }

        var userActivationDto = new UserActivationDto()
        {
            User = user.Adapt<UserDto>(),
            ActivationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user),
        };

        return ApplicationResult<UserActivationDto>.Success(userActivationDto);
    }

    public async Task<bool> HasRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> HasPermissionAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(
            principal,
            policyName
        );

        return result.Succeeded;
    }
}
