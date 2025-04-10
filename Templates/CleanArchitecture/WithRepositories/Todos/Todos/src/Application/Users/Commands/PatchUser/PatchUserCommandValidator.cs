using FluentValidation;

namespace Todos.Application.Users.Commands.PatchUser;

public class PatchUserCommandValidator : AbstractValidator<PatchUserCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public PatchUserCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;

        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => v.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(12)
            .When(v => v.UserName != null);

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
