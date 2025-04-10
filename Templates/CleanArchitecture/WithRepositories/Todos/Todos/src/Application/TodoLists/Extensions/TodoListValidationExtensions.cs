using FluentValidation;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoLists.Extensions;

public static class TodoListValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeUniqueTitle<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ITodoListRepository todoListRepository,
        ICurrentUser currentUser,
        Func<T, int> idSelector,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .MustAsync(
                async (obj, title, cancellationToken) =>
                {
                    int currentEntityId = idSelector(obj);

                    return !await todoListRepository.IsTitleInUseAsync(
                        title,
                        currentUser.Id ?? string.Empty,
                        TodoListId.From(currentEntityId),
                        cancellationToken
                    );
                }
            )
            .WithMessage(errorMessage ?? "'{PropertyName}' must be unique.");
    }

    public static IRuleBuilderOptions<T, string> MustBeUniqueTitle<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ITodoListRepository todoListRepository,
        ICurrentUser currentUser,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .MustAsync(
                async (title, cancellationToken) =>
                {
                    return !await todoListRepository.IsTitleInUseAsync(
                        title,
                        currentUser.Id ?? string.Empty,
                        TodoListId.From(0),
                        cancellationToken
                    );
                }
            )
            .WithMessage(errorMessage ?? "'{PropertyName}' must be unique.");
    }
}
