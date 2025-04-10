using FluentValidation.Results;

namespace Todos.Application.Shared.Extensions;

public static class ApplicationResultExtensions
{
    public static IEnumerable<ValidationFailure> ToValidationFailures<T>(
        this ApplicationResult<T> result
    )
    {
        return result.Errors.SelectMany(kvp =>
            kvp.Value.Select(error => new ValidationFailure(kvp.Key, error))
        );
    }

    public static IEnumerable<ValidationFailure> ToValidationFailures(this ApplicationResult result)
    {
        return result.Errors.SelectMany(kvp =>
            kvp.Value.Select(error => new ValidationFailure(kvp.Key, error))
        );
    }

    public static string FlattenErrors(this ApplicationResult result, string delimiter = ", ")
    {
        return string.Join(delimiter, result.Errors.SelectMany(kvp => kvp.Value));
    }

    public static string FlattenErrors<T>(this ApplicationResult<T> result, string delimiter = ", ")
    {
        return string.Join(delimiter, result.Errors.SelectMany(kvp => kvp.Value));
    }
}
