namespace Todos.Application.Shared.Models;

public class ApplicationResult
{
    internal ApplicationResult(bool succeeded, IEnumerable<KeyValuePair<string, string>> errors)
    {
        Succeeded = succeeded;
        Errors = errors
            .GroupBy(e => e.Key, e => e.Value)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    public bool Succeeded { get; init; }
    public IDictionary<string, string[]> Errors { get; init; }

    public static ApplicationResult Success()
    {
        return new ApplicationResult(true, []);
    }

    public static ApplicationResult Failure(IEnumerable<KeyValuePair<string, string>> errors)
    {
        return new ApplicationResult(false, errors);
    }

    public static ApplicationResult Failure(string errorCode, string error)
    {
        return new ApplicationResult(false, [new KeyValuePair<string, string>(errorCode, error)]);
    }
}

public class ApplicationResult<T> : ApplicationResult
{
    internal ApplicationResult(
        bool succeeded,
        IEnumerable<KeyValuePair<string, string>> errors,
        T? value = default
    )
        : base(succeeded, errors)
    {
        Value = value;
    }

    public T? Value { get; init; }

    public static ApplicationResult<T> Success(T value)
    {
        return new ApplicationResult<T>(true, [], value);
    }

    public static ApplicationResult<T> Failure(
        IEnumerable<KeyValuePair<string, string>> errors,
        T? value = default
    )
    {
        return new ApplicationResult<T>(false, errors, value);
    }

    public static ApplicationResult<T> Failure(string errorCode, string error, T? value = default)
    {
        return new ApplicationResult<T>(
            false,
            [new KeyValuePair<string, string>(errorCode, error)],
            value
        );
    }
}
