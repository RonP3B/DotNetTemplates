namespace Todos.Application.FileManager.Commands.DeleteFile;

[Authorize]
public record DeleteFileCommand(string ImageKey, string UserId) : IRequest;
