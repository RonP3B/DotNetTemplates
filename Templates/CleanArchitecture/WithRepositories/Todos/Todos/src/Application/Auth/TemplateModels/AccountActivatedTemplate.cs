namespace Todos.Application.Auth.TemplateModels;

public class AccountActivatedTemplate(string username) : TemplateModel
{
    public string Username { get; } = username;

    public override string TemplateName => nameof(AccountActivatedTemplate);

    public override FileType TemplateFileType => FileType.Html;

    public override Dictionary<string, string> GetPlaceholders()
    {
        return new() { { FormatPlaceholder(nameof(Username)), Username } };
    }
}
