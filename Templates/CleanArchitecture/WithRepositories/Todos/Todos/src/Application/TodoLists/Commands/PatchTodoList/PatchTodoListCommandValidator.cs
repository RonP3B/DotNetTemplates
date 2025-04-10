using FluentValidation;
using Todos.Application.TodoLists.Extensions;
using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Commands.PatchTodoList;

public class PatchTodoListCommandValidator : AbstractValidator<PatchTodoListCommand>
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUser _currentUser;

    public PatchTodoListCommandValidator(
        ITodoListRepository todoListRepository,
        IFileStorageService fileStorageService,
        ICurrentUser currentUser
    )
    {
        _todoListRepository = todoListRepository;
        _fileStorageService = fileStorageService;
        _currentUser = currentUser;

        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(100)
            .MustBeUniqueTitle(_todoListRepository, _currentUser, v => v.Id)
            .When(v => v.Title != null);

        RuleFor(v => v.Colour).NotEmpty().Length(7).When(v => v.Colour != null);

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
