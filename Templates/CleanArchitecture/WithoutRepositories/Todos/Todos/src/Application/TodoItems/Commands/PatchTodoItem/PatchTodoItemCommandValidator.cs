using FluentValidation;

namespace Todos.Application.TodoItems.Commands.PatchTodoItem;

public class PatchTodoItemCommandValidator : AbstractValidator<PatchTodoItemCommand>
{
    public PatchTodoItemCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => v.Title).MaximumLength(200).NotEmpty().When(v => v.Title != null);
    }
}
