namespace Todos.Application.Shared.Models;

public abstract class TemplateModel
{
    public abstract string TemplateName { get; }

    public abstract FileType TemplateFileType { get; }

    public abstract Dictionary<string, string> GetPlaceholders();

    public enum FileType
    {
        Html,
        // Add more file types as needed
    }

    protected static string FormatPlaceholder(string propertyName)
    {
        return $"{{{{{propertyName}}}}}";
    }
}
