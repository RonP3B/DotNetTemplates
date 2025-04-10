using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Commands.PatchTodoList;

[Authorize]
public record PatchTodoListCommand : IRequest<TodoListDto>
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public string? ImageKey { get; set; }
}
