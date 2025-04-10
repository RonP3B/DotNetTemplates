namespace Todos.Domain.TodoList;

public class TodoListEntity : BaseAuditableEntity<TodoListId>
{
    public required string Title { get; set; }

    public required Colour Colour { get; set; }

    public ImageKey? ImageKey { get; set; }

    public IList<TodoItemEntity> Items { get; private set; } = [];
}
