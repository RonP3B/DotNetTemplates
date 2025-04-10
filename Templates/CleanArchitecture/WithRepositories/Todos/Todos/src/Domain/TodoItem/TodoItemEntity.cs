namespace Todos.Domain.TodoItem;

public class TodoItemEntity : BaseAuditableEntity<TodoItemId>
{
    public required string Title { get; set; }

    public Note? Note { get; set; } = null;

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done = false;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public TodoListId ListId { get; set; } = default!;
    public TodoListEntity List { get; set; } = null!;
}
