using Todos.Application.Users.DTOs;

namespace Todos.Application.Users.Queries.GetUser;

public class GetUserQueryHandler(IApplicationUserManager applicationUserManager)
    : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;

    public async Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        return await _applicationUserManager.GetUserByUsernameAsync(query.Username);
    }
}
