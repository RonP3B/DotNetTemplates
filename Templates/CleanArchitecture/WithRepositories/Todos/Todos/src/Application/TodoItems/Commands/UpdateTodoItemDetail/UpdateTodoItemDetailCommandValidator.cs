using FluentValidation;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItemDetail;

public class UpdateTodoItemDetailCommandValidator : AbstractValidator<UpdateTodoItemDetailCommand>
{
    public UpdateTodoItemDetailCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => (int)v.Priority).InclusiveBetween(0, 3);

        RuleFor(v => v.Note).NotEmpty().MaximumLength(500).When(v => v.Note != null);
    }
}
