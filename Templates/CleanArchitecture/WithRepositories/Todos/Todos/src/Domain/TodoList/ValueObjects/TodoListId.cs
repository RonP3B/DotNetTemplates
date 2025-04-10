namespace Todos.Domain.TodoList.ValueObjects;

public class TodoListId : ValueObject
{
    private TodoListId(int value)
    {
        Value = value;
    }

    public static TodoListId From(int value)
    {
        return new TodoListId(value);
    }

    public int Value { get; private set; }

    public static implicit operator int(TodoListId id) => id.Value;

    public static explicit operator TodoListId(int value) => From(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
