namespace Todos.Domain.TodoItem.Events;

public class TodoItemCreatedEvent(TodoItemEntity item) : BaseEvent
{
    public TodoItemEntity Item { get; } = item;
}
