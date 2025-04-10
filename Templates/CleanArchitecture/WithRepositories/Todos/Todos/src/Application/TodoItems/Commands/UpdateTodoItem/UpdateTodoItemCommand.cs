using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItem;

[Authorize]
public record UpdateTodoItemCommand : IRequest<TodoItemDto>
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public bool Done { get; init; }
}
