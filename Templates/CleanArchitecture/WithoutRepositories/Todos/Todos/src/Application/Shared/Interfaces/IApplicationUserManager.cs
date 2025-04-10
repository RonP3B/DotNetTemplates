using Todos.Application.Users.Commands.CreateUser;
using Todos.Application.Users.Commands.PatchUser;
using Todos.Application.Users.Commands.UpdateUser;
using Todos.Application.Users.DTOs;

namespace Todos.Application.Shared.Interfaces;

public interface IApplicationUserManager
{
    Task<IEnumerable<UserDto>> GetUsersAsync();

    Task<UserDto> GetUserByUsernameAsync(string userName);

    Task<UserDto> GetUserByIdAsync(string userId);

    Task<ApplicationResult<CreatedUserDto>> CreateUserAsync(CreateUserCommand command);

    Task<ApplicationResult<UserDto>> UpdateUserAsync(UpdateUserCommand command);

    Task<ApplicationResult<UserDto>> PatchUserAsync(PatchUserCommand command);

    Task<ApplicationResult<UserDto>> DeleteUserAsync(string userId);
}
