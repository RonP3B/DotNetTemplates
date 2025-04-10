using System.Text.RegularExpressions;

namespace Todos.Domain.TodoList.ValueObjects;

public partial class ImageKey : ValueObject
{
    private ImageKey(string key)
    {
        Key = key;
    }

    public static ImageKey From(string key)
    {
        if (!ValidImageIdRegex().IsMatch(key))
        {
            throw new InvalidImageKeyException(nameof(ImageKey), key);
        }

        return new ImageKey(key);
    }

    public string Key { get; private set; }

    [GeneratedRegex(
        @"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}_
        [0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}_
        [\w-]+\.(jpg|jpeg|png|webp)$",
        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
    )]
    private static partial Regex ValidImageIdRegex();

    public static implicit operator string(ImageKey key) => key.ToString();

    public static explicit operator ImageKey(string key) => From(key);

    public override string ToString() => Key;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
    }
}
