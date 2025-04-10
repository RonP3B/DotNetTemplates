using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IApplicationUserManager applicationUserManager)
    : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;

    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _applicationUserManager.GetUsersAsync();
    }
}
