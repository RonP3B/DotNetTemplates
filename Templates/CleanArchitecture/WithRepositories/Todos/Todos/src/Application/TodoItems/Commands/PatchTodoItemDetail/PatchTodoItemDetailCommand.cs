using Todos.Application.TodoItems.DTOs;
using Todos.Domain.TodoItem.Enums;

namespace Todos.Application.TodoItems.Commands.PatchTodoItemDetail;

[Authorize]
public record PatchTodoItemDetailCommand : IRequest<TodoItemDto>
{
    public int Id { get; init; }

    public PriorityLevel? Priority { get; init; }

    public string? Note { get; init; }
}
