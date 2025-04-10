using Microsoft.AspNetCore.Http.HttpResults;
using Todos.Application.Users.Commands.CreateUser;
using Todos.Application.Users.Commands.DeleteUser;
using Todos.Application.Users.Commands.PatchUser;
using Todos.Application.Users.Commands.UpdateUser;
using Todos.Application.Users.DTOs;
using Todos.Application.Users.Queries.GetUser;
using Todos.Application.Users.Queries.GetUsers;

namespace Todos.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUsers)
            .MapGet(GetUser, "{username}")
            .MapPost(CreateUser)
            .MapPut(UpdateUser, "{userId}")
            .MapPatch(PatchUser, "{userId}")
            .MapDelete(DeleteUser, "{userId}");
    }

    public async Task<Ok<UserDto>> GetUser(ISender sender, string username)
    {
        UserDto user = await sender.Send(new GetUserQuery(username));

        return TypedResults.Ok(user);
    }

    public async Task<Ok<IEnumerable<UserDto>>> GetUsers(ISender sender)
    {
        IEnumerable<UserDto> users = await sender.Send(new GetUsersQuery());

        return TypedResults.Ok(users);
    }

    public async Task<Created<UserDto>> CreateUser(ISender sender, CreateUserCommand command)
    {
        UserDto createdUser = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Users)}/{createdUser.Id}", createdUser);
    }

    public async Task<Results<Ok<UserDto>, BadRequest>> UpdateUser(
        ISender sender,
        UpdateUserCommand command,
        string userId
    )
    {
        if (userId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        UserDto updatedUser = await sender.Send(command);

        return TypedResults.Ok(updatedUser);
    }

    public async Task<Results<Ok<UserDto>, BadRequest>> PatchUser(
        ISender sender,
        PatchUserCommand command,
        string userId
    )
    {
        if (userId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        UserDto patchedUser = await sender.Send(command);

        return TypedResults.Ok(patchedUser);
    }

    public async Task<NoContent> DeleteUser(ISender sender, string userId)
    {
        await sender.Send(new DeleteUserCommand(userId));

        return TypedResults.NoContent();
    }
}
