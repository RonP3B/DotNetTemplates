using FluentValidation;

namespace Todos.Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    public DeleteTodoItemCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}
