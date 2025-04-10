using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemBriefDto>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<PaginatedList<TodoItemBriefDto>> Handle(
        GetTodoItemsWithPaginationQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .TodoItems.Where(x => x.ListId == query.ListId)
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .ProjectToType<TodoItemBriefDto>()
            .PaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
}
