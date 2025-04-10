using Microsoft.AspNetCore.Http.HttpResults;
using Todos.Application.TodoLists.Commands.CreateTodoList;
using Todos.Application.TodoLists.Commands.DeleteTodoList;
using Todos.Application.TodoLists.Commands.PatchTodoList;
using Todos.Application.TodoLists.Commands.UpdateTodoList;
using Todos.Application.TodoLists.DTOs;
using Todos.Application.TodoLists.Queries.GetTodo;
using Todos.Application.TodoLists.Queries.GetTodos;
using Todos.Application.TodoLists.Queries.GetUserTodos;

namespace Todos.Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTodoLists)
            .MapGet(GetTodoList, "{todoListId}")
            .MapGet(GetUserTodoLists, "user/{userId}")
            .MapPost(CreateTodoList)
            .MapPut(UpdateTodoList, "{todoListId}")
            .MapPatch(PatchTodoList, "{todoListId}")
            .MapDelete(DeleteTodoList, "{todoListId}");
    }

    public async Task<Ok<TodoListDto>> GetTodoList(ISender sender, int todoListId)
    {
        TodoListDto todoList = await sender.Send(new GetTodoQuery(todoListId));

        return TypedResults.Ok(todoList);
    }

    public async Task<Ok<TodoListsWithPriorityDto>> GetTodoLists(ISender sender)
    {
        TodoListsWithPriorityDto todoLits = await sender.Send(new GetTodosQuery());

        return TypedResults.Ok(todoLits);
    }

    public async Task<Ok<TodoListsWithPriorityDto>> GetUserTodoLists(ISender sender, string userId)
    {
        TodoListsWithPriorityDto todoLists = await sender.Send(new GetUserTodosQuery(userId));

        return TypedResults.Ok(todoLists);
    }

    public async Task<Created<TodoListDto>> CreateTodoList(
        ISender sender,
        CreateTodoListCommand command
    )
    {
        TodoListDto createdTodoList = await sender.Send(command);

        return TypedResults.Created($"/{nameof(TodoLists)}/{createdTodoList.Id}", createdTodoList);
    }

    public async Task<Results<Ok<TodoListDto>, BadRequest>> UpdateTodoList(
        ISender sender,
        int todoListId,
        UpdateTodoListCommand command
    )
    {
        if (todoListId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoListDto todoList = await sender.Send(command);

        return TypedResults.Ok(todoList);
    }

    public async Task<Results<Ok<TodoListDto>, BadRequest>> PatchTodoList(
        ISender sender,
        int todoListId,
        PatchTodoListCommand command
    )
    {
        if (todoListId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoListDto todoList = await sender.Send(command);

        return TypedResults.Ok(todoList);
    }

    public async Task<NoContent> DeleteTodoList(ISender sender, int todoListId)
    {
        await sender.Send(new DeleteTodoListCommand(todoListId));

        return TypedResults.NoContent();
    }
}
