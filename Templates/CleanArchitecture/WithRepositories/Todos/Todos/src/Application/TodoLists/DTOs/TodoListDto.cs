using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoLists.DTOs;

public record TodoListDto
{
    public TodoListDto()
    {
        Items = [];
    }

    public int Id { get; init; }

    public required string Title { get; init; }

    public required string Colour { get; init; }

    public string? ImageKey { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }
}
