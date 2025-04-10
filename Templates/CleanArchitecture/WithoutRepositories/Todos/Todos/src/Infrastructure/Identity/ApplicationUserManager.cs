using Microsoft.AspNetCore.Identity;
using Todos.Application.Shared.Extensions;
using Todos.Application.Shared.Interfaces;
using Todos.Application.Shared.Models;
using Todos.Application.Users.Commands.CreateUser;
using Todos.Application.Users.Commands.PatchUser;
using Todos.Application.Users.Commands.UpdateUser;
using Todos.Application.Users.DTOs;

namespace Todos.Infrastructure.Identity;

public class ApplicationUserManager(UserManager<ApplicationUser> userManager)
    : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        List<ApplicationUser> users = await Task.FromResult(_userManager.Users.ToList());

        return users.Adapt<IEnumerable<UserDto>>();
    }

    public async Task<UserDto> GetUserByUsernameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        Guard.Against.NotFound(userName, user);

        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        Guard.Against.NotFound(userId, user);

        return user.Adapt<UserDto>();
    }

    public async Task<ApplicationResult<CreatedUserDto>> CreateUserAsync(CreateUserCommand command)
    {
        ApplicationUser user = command.Adapt<ApplicationUser>();

        IdentityResult identityResult = await _userManager.CreateAsync(user, command.Password);

        var createdUserDto = new CreatedUserDto
        {
            ActivationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user),
            User = user.Adapt<UserDto>(),
        };

        return identityResult.ToApplicationResult(createdUserDto);
    }

    public async Task<ApplicationResult<UserDto>> UpdateUserAsync(UpdateUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.Id);

        Guard.Against.NotFound(command.Id, user);

        command.Adapt(user);

        IdentityResult identityResult = await _userManager.UpdateAsync(user);

        return identityResult.ToApplicationResult(user.Adapt<UserDto>());
    }

    public async Task<ApplicationResult<UserDto>> PatchUserAsync(PatchUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.Id);

        Guard.Against.NotFound(command.Id, user);

        command.PartialAdapt(user);

        IdentityResult identityResult = await _userManager.UpdateAsync(user);

        return identityResult.ToApplicationResult(user.Adapt<UserDto>());
    }

    public async Task<ApplicationResult<UserDto>> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        Guard.Against.NotFound(userId, user);

        IdentityResult identityResult = await _userManager.DeleteAsync(user);

        return identityResult.ToApplicationResult(user.Adapt<UserDto>());
    }
}
