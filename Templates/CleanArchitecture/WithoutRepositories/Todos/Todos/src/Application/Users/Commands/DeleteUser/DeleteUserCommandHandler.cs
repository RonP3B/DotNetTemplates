using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IApplicationUserManager applicationUserManager)
    : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;

    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        ApplicationResult<UserDto> result = await _applicationUserManager.DeleteUserAsync(
            command.Id
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }
    }
}
