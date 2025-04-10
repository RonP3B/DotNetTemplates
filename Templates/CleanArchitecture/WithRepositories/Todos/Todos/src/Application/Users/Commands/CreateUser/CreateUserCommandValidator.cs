using FluentValidation;

namespace Todos.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public CreateUserCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;

        RuleFor(v => v.UserName).NotEmpty().MinimumLength(3).MaximumLength(15);

        RuleFor(v => v.Email)
            .NotEmpty()
            .Matches("^\\S+@\\S+\\.\\S+$")
            .WithMessage("Invalid email format.");

        RuleFor(v => v.Password).NotEmpty();

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService)
            .When(v => v.ImageKey != null);
    }
}
