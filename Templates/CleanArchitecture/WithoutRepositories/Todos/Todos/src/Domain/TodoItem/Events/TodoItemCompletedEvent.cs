namespace Todos.Domain.TodoItem.Events;

public class TodoItemCompletedEvent(TodoItemEntity item) : BaseEvent
{
    public TodoItemEntity Item { get; } = item;
}
