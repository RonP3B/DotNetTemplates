using Todos.Application.FileManager.DTOs;

namespace Todos.Application.FileManager.Commands.UploadFile;

[Authorize]
public record UploadFileCommand(FileDataDto FileData) : IRequest<UploadedFileDto>;
