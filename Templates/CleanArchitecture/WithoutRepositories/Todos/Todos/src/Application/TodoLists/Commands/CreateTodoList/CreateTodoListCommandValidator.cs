using FluentValidation;
using Todos.Application.TodoLists.Extensions;

namespace Todos.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUser _currentUser;

    public CreateTodoListCommandValidator(
        IApplicationDbContext context,
        IFileStorageService fileStorageService,
        ICurrentUser currentUser
    )
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _currentUser = currentUser;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustBeUniqueTitle(_context, _currentUser);

        RuleFor(v => v.Colour).NotEmpty().Length(7);

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
