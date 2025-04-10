using Todos.Domain.Shared.Constants;

namespace Todos.Application.Users.Commands.DeleteUser;

[Authorize(Roles = Roles.Administrator)]
public record DeleteUserCommand(string Id) : IRequest;
