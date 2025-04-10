using Todos.Application.FileManager.DTOs;

namespace Todos.Application.FileManager.Commands.UploadFiles;

[Authorize]
public record UploadFilesCommand(List<FileDataDto> FilesData)
    : IRequest<IEnumerable<UploadedFileDto>>;
