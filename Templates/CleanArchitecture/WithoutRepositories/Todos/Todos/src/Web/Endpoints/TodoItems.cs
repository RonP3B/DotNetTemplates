using Microsoft.AspNetCore.Http.HttpResults;
using Todos.Application.Shared.Models;
using Todos.Application.TodoItems.Commands.CreateTodoItem;
using Todos.Application.TodoItems.Commands.DeleteTodoItem;
using Todos.Application.TodoItems.Commands.PatchTodoItem;
using Todos.Application.TodoItems.Commands.PatchTodoItemDetail;
using Todos.Application.TodoItems.Commands.UpdateTodoItem;
using Todos.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Todos.Application.TodoItems.DTOs;
using Todos.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace Todos.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTodoItemsWithPagination)
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{todoItemId}")
            .MapPut(UpdateTodoItemDetail, "detail/{todoItemId}")
            .MapPatch(PatchTodoItem, "{todoItemId}")
            .MapPatch(PatchTodoItemDetail, "detail/{todoItemId}")
            .MapDelete(DeleteTodoItem, "{todoItemId}");
    }

    public async Task<Ok<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination(
        ISender sender,
        [AsParameters] GetTodoItemsWithPaginationQuery query
    )
    {
        PaginatedList<TodoItemBriefDto> todoItems = await sender.Send(query);

        return TypedResults.Ok(todoItems);
    }

    public async Task<Created<TodoItemDto>> CreateTodoItem(
        ISender sender,
        CreateTodoItemCommand command
    )
    {
        TodoItemDto createdTodoItem = await sender.Send(command);

        return TypedResults.Created($"/{nameof(TodoItems)}/{createdTodoItem.Id}", createdTodoItem);
    }

    public async Task<Results<Ok<TodoItemDto>, BadRequest>> UpdateTodoItem(
        ISender sender,
        int todoItemId,
        UpdateTodoItemCommand command
    )
    {
        if (todoItemId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoItemDto updatedTodoItem = await sender.Send(command);

        return TypedResults.Ok(updatedTodoItem);
    }

    public async Task<Results<Ok<TodoItemDto>, BadRequest>> UpdateTodoItemDetail(
        ISender sender,
        int todoItemId,
        UpdateTodoItemDetailCommand command
    )
    {
        if (todoItemId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoItemDto updatedTodoItem = await sender.Send(command);

        return TypedResults.Ok(updatedTodoItem);
    }

    public async Task<Results<Ok<TodoItemDto>, BadRequest>> PatchTodoItem(
        ISender sender,
        int todoItemId,
        PatchTodoItemCommand command
    )
    {
        if (todoItemId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoItemDto patchedTodoItem = await sender.Send(command);

        return TypedResults.Ok(patchedTodoItem);
    }

    public async Task<Results<Ok<TodoItemDto>, BadRequest>> PatchTodoItemDetail(
        ISender sender,
        int todoItemId,
        PatchTodoItemDetailCommand command
    )
    {
        if (todoItemId != command.Id)
        {
            return TypedResults.BadRequest();
        }

        TodoItemDto patchedTodoItem = await sender.Send(command);

        return TypedResults.Ok(patchedTodoItem);
    }

    public async Task<NoContent> DeleteTodoItem(ISender sender, int todoItemId)
    {
        await sender.Send(new DeleteTodoItemCommand(todoItemId));

        return TypedResults.NoContent();
    }
}
