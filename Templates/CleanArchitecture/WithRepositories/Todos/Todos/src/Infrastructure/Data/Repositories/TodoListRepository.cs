using Microsoft.EntityFrameworkCore;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;
using Todos.Infrastructure.Data.Contexts;

namespace Todos.Infrastructure.Data.Repositories;

public class TodoListRepository : BaseRepository<TodoListEntity, TodoListId>, ITodoListRepository
{
    public TodoListRepository(ApplicationDbContext context)
        : base(context) { }

    public override Task<TodoListEntity?> GetAsync(
        TodoListId id,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .TodoLists.Include(l => l.Items)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public override Task<TodoListEntity?> GetForReadOnlyAsync(
        TodoListId id,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .TodoLists.AsNoTracking()
            .Include(l => l.Items)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<TodoListEntity?> GetByCreatorAsync(
        TodoListId id,
        string createdBy,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .TodoLists.Where(l => l.Id == id && l.CreatedBy == createdBy)
            .Include(l => l.Items)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<TodoListEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context
            .TodoLists.AsNoTracking()
            .Include(l => l.Items)
            .OrderBy(l => l.Title)
            .ToListAsync(cancellationToken);
    }

    public Task<List<TodoListEntity>> GetAllByCreatorAsync(
        string createdBy,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .TodoLists.Where(l => l.CreatedBy == createdBy)
            .Include(l => l.Items)
            .AsNoTracking()
            .OrderBy(l => l.Title)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsTitleInUseAsync(
        string title,
        string createdBy,
        TodoListId currentTodoListId,
        CancellationToken cancellationToken = default
    )
    {
        return _context.TodoLists.AnyAsync(
            l => l.Title == title && l.CreatedBy == createdBy && currentTodoListId != l.Id,
            cancellationToken
        );
    }

    public void Purge()
    {
        _context.TodoLists.RemoveRange(_context.TodoLists);
    }
}
