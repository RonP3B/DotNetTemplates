using Todos.Application.TodoItems.DTOs;
using Todos.Domain.TodoItem.Enums;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItemDetail;

[Authorize]
public record UpdateTodoItemDetailCommand : IRequest<TodoItemDto>
{
    public int Id { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}
