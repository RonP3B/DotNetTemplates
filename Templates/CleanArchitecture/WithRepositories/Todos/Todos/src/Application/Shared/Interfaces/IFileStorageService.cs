namespace Todos.Application.Shared.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<string>> SaveFilesAsync(
        IEnumerable<(Stream FileStream, string FileName)> files,
        CancellationToken cancellationToken = default
    );

    Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
}
