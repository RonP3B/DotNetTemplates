using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Commands.UpdateTodoList;

[Authorize]
public record UpdateTodoListCommand : IRequest<TodoListDto>
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public required string Colour { get; init; }

    public string? ImageKey { get; set; }
}
