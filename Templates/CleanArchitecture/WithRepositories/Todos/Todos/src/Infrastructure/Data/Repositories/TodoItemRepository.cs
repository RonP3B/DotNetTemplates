using Microsoft.EntityFrameworkCore;
using Todos.Domain.Shared.ResultTypes;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.ValueObjects;
using Todos.Domain.TodoList.ValueObjects;
using Todos.Infrastructure.Data.Contexts;

namespace Todos.Infrastructure.Data.Repositories;

public class TodoItemRepository : BaseRepository<TodoItemEntity, TodoItemId>, ITodoItemRepository
{
    public TodoItemRepository(ApplicationDbContext context)
        : base(context) { }

    public Task<TodoItemEntity?> GetByCreatorAsync(
        TodoItemId id,
        string createdBy,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .TodoItems.Where(i => i.Id == id && i.CreatedBy == createdBy)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<TodoItemEntity>> GetPagedByListIdAsync(
        TodoListId listId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var paginatedTodoItems = await _context
            .TodoItems.Where(x => x.ListId == listId)
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .PaginatedListAsync(pageNumber, pageSize, cancellationToken);

        return new PagedResult<TodoItemEntity>()
        {
            Items = paginatedTodoItems.Items,
            TotalCount = paginatedTodoItems.TotalCount,
            PageNumber = paginatedTodoItems.PageNumber,
            TotalPages = paginatedTodoItems.TotalPages,
        };
    }
}
