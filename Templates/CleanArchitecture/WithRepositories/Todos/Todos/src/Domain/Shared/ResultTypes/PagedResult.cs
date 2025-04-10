namespace Todos.Domain.Shared.ResultTypes;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int TotalCount { get; init; }

    public int PageNumber { get; init; }

    public int TotalPages { get; init; }
}
