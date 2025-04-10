namespace Todos.Application.Auth.Commands.ChangePassword;

[Authorize]
public record ChangePasswordCommand : IRequest
{
    public required string UserId { get; init; }

    public required string CurrentPassword { get; init; }

    public required string NewPassword { get; init; }
}
