using Todos.Application.TodoItems.DTOs;
using Todos.Domain.Shared.ResultTypes;

namespace Todos.Application.TodoItems.Queries.GetTodoItemsWithPagination;

[Authorize]
public record GetTodoItemsWithPaginationQuery : IRequest<PagedResult<TodoItemBriefDto>>
{
    public int ListId { get; init; }

    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
