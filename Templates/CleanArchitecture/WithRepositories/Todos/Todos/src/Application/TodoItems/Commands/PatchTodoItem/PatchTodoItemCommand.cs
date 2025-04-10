using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Commands.PatchTodoItem;

[Authorize]
public record PatchTodoItemCommand : IRequest<TodoItemDto>
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool? Done { get; init; }
}
