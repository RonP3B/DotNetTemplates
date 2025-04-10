namespace Todos.Domain.TodoList;

public interface ITodoListRepository
{
    Task<TodoListEntity?> GetAsync(TodoListId id, CancellationToken cancellationToken = default);

    Task<TodoListEntity?> GetForReadOnlyAsync(
        TodoListId id,
        CancellationToken cancellationToken = default
    );

    Task<TodoListEntity?> GetByCreatorAsync(
        TodoListId id,
        string createdBy,
        CancellationToken cancellationToken = default
    );

    Task<List<TodoListEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<TodoListEntity>> GetAllByCreatorAsync(
        string createdBy,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsTitleInUseAsync(
        string title,
        string createdBy,
        TodoListId currentTodoListId,
        CancellationToken cancellationToken = default
    );

    void Add(TodoListEntity todoList);

    void Update(TodoListEntity todoList);

    void Remove(TodoListEntity todoList);

    void Purge();
}
