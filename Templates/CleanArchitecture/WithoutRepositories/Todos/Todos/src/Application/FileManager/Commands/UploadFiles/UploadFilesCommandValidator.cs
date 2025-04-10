using FluentValidation;
using Todos.Application.FileManager.Extensions;

namespace Todos.Application.FileManager.Commands.UploadFiles;

public class UploadFilesCommandValidator : AbstractValidator<UploadFilesCommand>
{
    public UploadFilesCommandValidator()
    {
        RuleFor(v => v.FilesData)
            .NotEmpty()
            .WithMessage("At least one file must be provided.")
            .Must(files => files.Count <= 5)
            .WithMessage("Maximum 5 files can be uploaded at once.");

        RuleForEach(v => v.FilesData)
            .ChildRules(file =>
            {
                file.RuleFor(f => f.FileName).NotEmpty().MaximumLength(255);

                file.RuleFor(f => f.ContentType).NotEmpty().MustBeValidFileType();

                file.RuleFor(f => f.Content)
                    .NotNull()
                    .When(f =>
                        !string.IsNullOrEmpty(f.ContentType)
                        && FileValidationExtensions.IsValidFileType(f.ContentType)
                    );
            });
    }
}
