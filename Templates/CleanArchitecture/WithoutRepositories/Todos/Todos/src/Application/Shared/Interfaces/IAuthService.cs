using Todos.Application.Auth.DTOs;
using Todos.Application.Users.DTOs;

namespace Todos.Application.Shared.Interfaces;

public interface IAuthService
{
    Task<ApplicationResult<UserActivationDto>> GetActivationTokenAsync(string userName);

    Task<PasswordResetDto> GetPasswordResetTokenAsync(string userName);

    Task<ApplicationResult> ActivateAccountAsync(string userName, string activationToken);

    Task<ApplicationResult> ResetPasswordAsync(
        string userId,
        string resetToken,
        string newPassword
    );

    Task<ApplicationResult> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword
    );

    Task<ApplicationResult<UserDto>> ValidateUserAsync(string userName, string password);

    Task<bool> HasRoleAsync(string userId, string role);

    Task<bool> HasPermissionAsync(string userId, string policyName);
}
