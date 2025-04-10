namespace Todos.Application.TodoLists.TemplateModels;

public class TodoListUpdatedTemplate(string username, string listTitle, string lastModified)
    : TemplateModel
{
    public string Username { get; } = username;

    public string ListTitle { get; } = listTitle;

    public string LastModified { get; } = lastModified;

    public override string TemplateName => nameof(TodoListUpdatedTemplate);

    public override FileType TemplateFileType => FileType.Html;

    public override Dictionary<string, string> GetPlaceholders()
    {
        return new()
        {
            { FormatPlaceholder(nameof(Username)), Username },
            { FormatPlaceholder(nameof(ListTitle)), ListTitle },
            { FormatPlaceholder(nameof(LastModified)), LastModified },
        };
    }
}
