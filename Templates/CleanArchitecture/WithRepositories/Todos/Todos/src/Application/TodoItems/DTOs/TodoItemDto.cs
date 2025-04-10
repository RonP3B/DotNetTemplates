namespace Todos.Application.TodoItems.DTOs;

public class TodoItemDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public required string Title { get; init; }

    public bool Done { get; init; }

    public int Priority { get; init; }

    public string? Note { get; init; }
}
