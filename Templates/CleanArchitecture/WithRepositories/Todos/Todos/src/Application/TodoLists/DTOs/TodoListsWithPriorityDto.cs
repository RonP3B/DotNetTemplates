namespace Todos.Application.TodoLists.DTOs;

public record TodoListsWithPriorityDto
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = [];

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = [];
}
