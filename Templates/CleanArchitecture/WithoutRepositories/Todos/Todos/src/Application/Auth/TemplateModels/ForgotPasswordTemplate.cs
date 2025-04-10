namespace Todos.Application.Auth.TemplateModels;

public class ForgotPasswordTemplate(string username, string resetLink) : TemplateModel
{
    public string Username { get; } = username;

    public string ResetLink { get; } = resetLink;

    public override string TemplateName => nameof(ForgotPasswordTemplate);

    public override FileType TemplateFileType => FileType.Html;

    public override Dictionary<string, string> GetPlaceholders()
    {
        return new()
        {
            { FormatPlaceholder(nameof(Username)), Username },
            { FormatPlaceholder(nameof(ResetLink)), ResetLink },
        };
    }
}
