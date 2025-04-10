using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Queries.GetTodoItemsWithPagination;

[Authorize]
public record GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemBriefDto>>
{
    public int ListId { get; init; }

    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
