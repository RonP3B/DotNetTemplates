namespace Todos.Application.FileManager.Commands.DeleteFile;

public class DeleteFileCommandHandler(
    IFileStorageService fileStorageService,
    ICurrentUser currentUser
) : IRequestHandler<DeleteFileCommand>
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task Handle(DeleteFileCommand command, CancellationToken cancellationToken)
    {
        if (_currentUser.Id != command.UserId)
        {
            throw new ForbiddenAccessException();
        }

        await _fileStorageService.DeleteFileAsync(command.ImageKey, cancellationToken);
    }
}
