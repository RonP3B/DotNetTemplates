namespace Todos.Domain.TodoItem.Events;

public class TodoItemDeletedEvent(TodoItemEntity item) : BaseEvent
{
    public TodoItemEntity Item { get; } = item;
}
