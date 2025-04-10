using FluentValidation;

namespace Todos.Application.FileManager.Extensions;

public static class FileValidationExtensions
{
    private static readonly Dictionary<string, long> MaxFileSizes =
        new()
        {
            { "image/jpeg", 2 * 1024 * 1024 }, // 2 MB for JPEG / JPG
            { "image/png", 2 * 1024 * 1024 }, // 2 MB for PNG
            {
                "image/webp",
                2 * 1024 * 1024
            } // 2 MB for WEBP
            ,
            // Add other MIME types and their limits here
        };

    public static IRuleBuilderOptions<T, string> MustBeValidFileType<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .Must(contentType => IsValidFileType(contentType))
            .WithMessage(errorMessage ?? "Unsupported file type.");
    }

    public static IRuleBuilderOptions<T, Stream> MustBeValidFileSize<T>(
        this IRuleBuilder<T, Stream> ruleBuilder,
        Func<T, string> contentTypeProvider,
        string? errorMessage = null
    )
    {
        return ruleBuilder
            .Must(
                (instance, stream, context) =>
                {
                    string contentType = contentTypeProvider(instance);

                    return IsValidFileType(contentType)
                        && stream.Length <= MaxFileSizes[contentType];
                }
            )
            .WithMessage(errorMessage ?? "File size exceeds the limit for this file type.");
    }

    public static bool IsValidFileType(string contentType)
    {
        return MaxFileSizes.ContainsKey(contentType);
    }
}
