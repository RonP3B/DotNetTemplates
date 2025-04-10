using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Commands.CreateTodoList;

[Authorize]
public record CreateTodoListCommand : IRequest<TodoListDto>
{
    public required string Title { get; init; }

    public required string Colour { get; init; }

    public string? ImageKey { get; init; }
}
