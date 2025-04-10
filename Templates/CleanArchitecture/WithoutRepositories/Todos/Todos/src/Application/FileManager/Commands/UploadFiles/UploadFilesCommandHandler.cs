using Todos.Application.FileManager.DTOs;

namespace Todos.Application.FileManager.Commands.UploadFiles;

public class UploadFilesCommandHandler(IFileStorageService fileStorageService)
    : IRequestHandler<UploadFilesCommand, IEnumerable<UploadedFileDto>>
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<IEnumerable<UploadedFileDto>> Handle(
        UploadFilesCommand command,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<(Stream, string)> files = command.FilesData.Select(f =>
            (f.Content, f.FileName)
        );

        IEnumerable<string> imageKeys = await _fileStorageService.SaveFilesAsync(
            files,
            cancellationToken
        );

        return imageKeys.Select(imageKey => new UploadedFileDto(imageKey));
    }
}
