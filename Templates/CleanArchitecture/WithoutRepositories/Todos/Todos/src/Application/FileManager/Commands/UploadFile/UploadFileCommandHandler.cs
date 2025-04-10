using Todos.Application.FileManager.DTOs;

namespace Todos.Application.FileManager.Commands.UploadFile;

public class UploadFileCommandHandler(IFileStorageService fileStorageService)
    : IRequestHandler<UploadFileCommand, UploadedFileDto>
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<UploadedFileDto> Handle(
        UploadFileCommand command,
        CancellationToken cancellationToken
    )
    {
        string imageKey = await _fileStorageService.SaveFileAsync(
            command.FileData.Content,
            command.FileData.FileName,
            cancellationToken
        );

        return new UploadedFileDto(imageKey);
    }
}
