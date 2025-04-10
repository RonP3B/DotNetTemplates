namespace Todos.Application.Users.TemplateModels;

public class AccountActivationTemplate(string username, string activationLink) : TemplateModel
{
    public string Username { get; } = username;

    public string ActivationLink { get; } = activationLink;

    public override string TemplateName => nameof(AccountActivationTemplate);

    public override FileType TemplateFileType => FileType.Html;

    public override Dictionary<string, string> GetPlaceholders()
    {
        return new()
        {
            { FormatPlaceholder(nameof(Username)), Username },
            { FormatPlaceholder(nameof(ActivationLink)), ActivationLink },
        };
    }
}
