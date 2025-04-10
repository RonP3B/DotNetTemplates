namespace Todos.Domain.TodoItem.ValueObjects;

public class TodoItemId : ValueObject
{
    private TodoItemId(int value)
    {
        Value = value;
    }

    public static TodoItemId From(int value)
    {
        return new TodoItemId(value);
    }

    public int Value { get; private set; }

    public static implicit operator int(TodoItemId id) => id.Value;

    public static explicit operator TodoItemId(int value) => From(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
