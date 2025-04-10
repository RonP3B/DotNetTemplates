using FluentValidation;

namespace Todos.Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommandValidator : AbstractValidator<DeleteTodoListCommand>
{
    public DeleteTodoListCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}
