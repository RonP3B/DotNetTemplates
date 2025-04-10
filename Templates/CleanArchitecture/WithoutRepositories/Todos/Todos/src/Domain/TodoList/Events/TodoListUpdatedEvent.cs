namespace Todos.Domain.TodoList.Events;

public class TodoListUpdatedEvent(TodoListEntity list) : BaseEvent
{
    public TodoListEntity TodoList { get; } = list;
}
