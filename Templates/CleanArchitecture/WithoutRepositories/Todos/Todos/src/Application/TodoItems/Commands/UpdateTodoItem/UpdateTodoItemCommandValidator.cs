using FluentValidation;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => v.Title).MaximumLength(200).NotEmpty();
    }
}
