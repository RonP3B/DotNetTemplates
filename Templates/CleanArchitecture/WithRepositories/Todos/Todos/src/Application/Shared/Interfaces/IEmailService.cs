namespace Todos.Application.Shared.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string address, string subject, string body, bool isHtml);
}
