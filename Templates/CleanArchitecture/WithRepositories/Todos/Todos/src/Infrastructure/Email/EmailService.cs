using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Todos.Application.Shared.Interfaces;

namespace Todos.Infrastructure.Email;

public class EmailService(IOptions<EmailSettings> mailSettings, ILogger<EmailService> logger)
    : IEmailService
{
    private readonly EmailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(
        string to,
        string address,
        string subject,
        string body,
        bool isHtml
    )
    {
        try
        {
            MimeMessage message = new();

            message.From.Add(new MailboxAddress(_mailSettings.FromName, _mailSettings.Username));

            message.To.Add(new MailboxAddress(to, address));

            message.Subject = subject;

            message.Body = new TextPart(isHtml ? "html" : "plain") { Text = body };

            using SmtpClient client = new();

            await client.ConnectAsync(
                _mailSettings.SmtpServer,
                _mailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "[Email Failure] Unable to send email | Recipient: '{Recipient}' | Address: '{Address}' | Subject: '{Subject}' | SMTP: {SmtpServer}:{Port}",
                to,
                address,
                subject,
                _mailSettings.SmtpServer,
                _mailSettings.Port
            );
        }
    }
}
