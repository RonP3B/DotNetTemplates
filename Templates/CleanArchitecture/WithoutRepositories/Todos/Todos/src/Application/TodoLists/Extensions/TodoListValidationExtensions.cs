using FluentValidation;

namespace Todos.Application.TodoLists.Extensions;

public static class TodoListValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeUniqueTitle<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IApplicationDbContext context,
        ICurrentUser currentUser,
        Func<T, int> idSelector,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .MustAsync(
                async (obj, title, cancellationToken) =>
                {
                    int currentTodoListId = idSelector(obj);

                    return !await context.TodoLists.AnyAsync(
                        l =>
                            l.Title == title
                            && l.CreatedBy == currentUser.Id
                            && l.Id != currentTodoListId,
                        cancellationToken
                    );
                }
            )
            .WithMessage(errorMessage ?? "'{PropertyName}' must be unique.");
    }

    public static IRuleBuilderOptions<T, string> MustBeUniqueTitle<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IApplicationDbContext context,
        ICurrentUser currentUser,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .MustAsync(
                async (title, cancellationToken) =>
                {
                    return !await context.TodoLists.AnyAsync(
                        l => l.Title == title && l.CreatedBy == currentUser.Id,
                        cancellationToken
                    );
                }
            )
            .WithMessage(errorMessage ?? "'{PropertyName}' must be unique.");
    }
}
