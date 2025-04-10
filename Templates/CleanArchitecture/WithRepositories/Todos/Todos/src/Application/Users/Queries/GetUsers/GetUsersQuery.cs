using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Queries.GetUsers;

[Authorize]
public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;
