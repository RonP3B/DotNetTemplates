using FluentValidation;

namespace Todos.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title).MinimumLength(3).MaximumLength(150).NotEmpty();
    }
}
