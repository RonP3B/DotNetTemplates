using Microsoft.AspNetCore.Hosting;
using Todos.Application.Shared.Interfaces;

namespace Todos.Infrastructure.FileServices;

public class ApiFileStorage : IFileStorageService
{
    private readonly string _uploadPath;
    private readonly ICurrentUser _currentUser;

    public ApiFileStorage(IWebHostEnvironment env, ICurrentUser currentUser)
    {
        _uploadPath = Path.Combine(env.WebRootPath, "uploads");
        _currentUser = currentUser;

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> SaveFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        string imageIdentifier = _currentUser.Id + "_" + Guid.NewGuid().ToString() + "_" + fileName;

        string filePath = Path.Combine(_uploadPath, imageIdentifier);

        using (FileStream fileStreamToWrite = new(filePath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileStreamToWrite, cancellationToken);
        }

        return imageIdentifier;
    }

    public async Task<IEnumerable<string>> SaveFilesAsync(
        IEnumerable<(Stream FileStream, string FileName)> files,
        CancellationToken cancellationToken = default
    )
    {
        List<string> savedFiles = [];

        IEnumerable<Task<string>> tasks = files.Select(async file =>
        {
            string imageIdentifier =
                _currentUser.Id + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;

            string filePath = Path.Combine(_uploadPath, imageIdentifier);

            using (FileStream fileStreamToWrite = new(filePath, FileMode.Create, FileAccess.Write))
            {
                await file.FileStream.CopyToAsync(fileStreamToWrite, cancellationToken);
            }

            return imageIdentifier;
        });

        savedFiles.AddRange(await Task.WhenAll(tasks));

        return savedFiles;
    }

    public async Task<bool> DeleteFileAsync(
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        string filePath = Path.Combine(_uploadPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public Task<bool> FileExistsAsync(
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        string filePath = Path.Combine(_uploadPath, fileName);

        bool exists = File.Exists(filePath);

        return Task.FromResult(exists);
    }
}
