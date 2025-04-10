using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Commands.CreateTodoItem;

[Authorize]
public record CreateTodoItemCommand : IRequest<TodoItemDto>
{
    public int ListId { get; init; }

    public required string Title { get; init; }
}
