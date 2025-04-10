using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Queries.GetUser;

[Authorize]
public record GetUserQuery(string Username) : IRequest<UserDto>;
