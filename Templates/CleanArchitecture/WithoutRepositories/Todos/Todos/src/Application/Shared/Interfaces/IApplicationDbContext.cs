using Todos.Domain.TodoItem;
using Todos.Domain.TodoList;

namespace Todos.Application.Shared.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoListEntity> TodoLists { get; }

    DbSet<TodoItemEntity> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
