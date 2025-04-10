using FluentValidation;

namespace Todos.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public UpdateUserCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;

        RuleFor(v => v.Id).NotEmpty();

        RuleFor(v => v.UserName).NotEmpty().MinimumLength(3).MaximumLength(12);

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
