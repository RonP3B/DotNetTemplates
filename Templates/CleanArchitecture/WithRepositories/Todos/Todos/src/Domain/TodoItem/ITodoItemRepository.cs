namespace Todos.Domain.TodoItem;

public interface ITodoItemRepository
{
    Task<TodoItemEntity?> GetAsync(TodoItemId id, CancellationToken cancellationToken = default);

    Task<TodoItemEntity?> GetForReadOnlyAsync(
        TodoItemId id,
        CancellationToken cancellationToken = default
    );

    Task<TodoItemEntity?> GetByCreatorAsync(
        TodoItemId id,
        string createdBy,
        CancellationToken cancellationToken = default
    );

    Task<PagedResult<TodoItemEntity>> GetPagedByListIdAsync(
        TodoListId todoListId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    void Add(TodoItemEntity todoItem);

    void Update(TodoItemEntity todoItem);

    void Remove(TodoItemEntity todoItem);
}
