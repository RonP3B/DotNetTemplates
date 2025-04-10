namespace Todos.Application.FileManager.DTOs;

public record FileDataDto(string FileName, string ContentType, Stream Content)
{
    public string FileName { get; } = FileName;

    public string ContentType { get; } = ContentType;

    public Stream Content { get; } = Content;
}
