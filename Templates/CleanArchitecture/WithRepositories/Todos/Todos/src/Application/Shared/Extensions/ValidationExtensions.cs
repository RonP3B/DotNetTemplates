using FluentValidation;

namespace Todos.Application.Shared.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> MustExistInFileStorage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        IFileStorageService fileStorageService,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .MustAsync(
                async (imageKey, cancellationToken) =>
                {
                    if (imageKey == null)
                    {
                        return false;
                    }

                    return await fileStorageService.FileExistsAsync(imageKey, cancellationToken);
                }
            )
            .WithMessage(
                errorMessage ?? "The file '{PropertyValue}' does not exist in our file storage."
            );
    }
}
