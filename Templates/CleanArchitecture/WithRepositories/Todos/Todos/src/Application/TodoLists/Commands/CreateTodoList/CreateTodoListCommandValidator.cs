using FluentValidation;
using Todos.Application.TodoLists.Extensions;
using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUser _currentUser;

    public CreateTodoListCommandValidator(
        ITodoListRepository todoListRepository,
        IFileStorageService fileStorageService,
        ICurrentUser currentUser
    )
    {
        _todoListRepository = todoListRepository;
        _fileStorageService = fileStorageService;
        _currentUser = currentUser;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustBeUniqueTitle(_todoListRepository, _currentUser);

        RuleFor(v => v.Colour).NotEmpty().Length(7);

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
