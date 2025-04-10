namespace Todos.Domain.TodoItem;

public class TodoItemEntity : BaseAuditableEntity
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

    public int ListId { get; set; }
    public TodoListEntity List { get; set; } = null!;
}
