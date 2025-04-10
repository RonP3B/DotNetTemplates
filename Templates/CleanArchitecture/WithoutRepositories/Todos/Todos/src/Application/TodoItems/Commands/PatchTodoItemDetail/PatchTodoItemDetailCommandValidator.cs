using FluentValidation;

namespace Todos.Application.TodoItems.Commands.PatchTodoItemDetail;

public class PatchTodoItemDetailCommandValidator : AbstractValidator<PatchTodoItemDetailCommand>
{
    public PatchTodoItemDetailCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => (int)v.Priority!).InclusiveBetween(0, 3).When(v => v.Priority != null);

        RuleFor(v => v.Note).NotEmpty().MaximumLength(500).When(v => v.Note != null);
    }
}
