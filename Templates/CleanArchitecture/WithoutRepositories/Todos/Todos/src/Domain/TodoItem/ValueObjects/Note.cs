using Todos.Domain.TodoItem.Exceptions;

namespace Todos.Domain.TodoItem.ValueObjects;

public class Note : ValueObject
{
    private const int MaxLength = 500;

    private Note(string content)
    {
        Content = content;
    }

    public static Note From(string content)
    {
        if (ForbiddenWords.Any(word => content.Contains(word, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ForbiddenNoteContentException(nameof(Note));
        }

        if (content.Length > MaxLength)
        {
            throw new NoteTooLongException(nameof(Note), MaxLength);
        }

        return new Note(content);
    }

    public string Content { get; private set; }

    public static implicit operator string(Note note) => note.ToString();

    public static explicit operator Note(string content) => From(content);

    public override string ToString() => Content;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Content;
    }

    private static readonly HashSet<string> ForbiddenWords =
    [
        "freedom",
        "democracy",
        "protest",
        "Taiwan independence",
        "Winnie Pooh",
    ];
}
