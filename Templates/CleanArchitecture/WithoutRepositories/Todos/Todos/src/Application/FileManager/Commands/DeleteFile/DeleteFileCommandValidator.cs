using FluentValidation;

namespace Todos.Application.FileManager.Commands.DeleteFile;

public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public DeleteFileCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;

        RuleFor(v => v.ImageKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustExistInFileStorage(_fileStorageService);

        RuleFor(v => v.UserId).NotEmpty();
    }
}
