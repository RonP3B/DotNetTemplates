using FluentValidation;
using Todos.Application.FileManager.Extensions;

namespace Todos.Application.FileManager.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(v => v.FileData.FileName).NotEmpty().MaximumLength(255);

        RuleFor(v => v.FileData.ContentType).NotEmpty().MustBeValidFileType();

        RuleFor(v => v.FileData.Content)
            .NotNull()
            .MustBeValidFileSize(v => v.FileData.ContentType)
            .When(x =>
                !string.IsNullOrEmpty(x.FileData.ContentType)
                && FileValidationExtensions.IsValidFileType(x.FileData.ContentType)
            );
    }
}
