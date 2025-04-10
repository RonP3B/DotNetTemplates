using Todos.Application.Auth.DTOs;
using Todos.Application.Auth.TemplateModels;

namespace Todos.Application.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IAuthService authService,
    IEmailService emailService,
    IAppSettings appSettings,
    ITemplateService templateService
) : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly IEmailService _emailService = emailService;
    private readonly IAppSettings _appSettings = appSettings;
    private readonly ITemplateService _templateService = templateService;

    public async Task Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        PasswordResetDto passwordResetDto = await _authService.GetPasswordResetTokenAsync(
            command.Username
        );

        string resetLink = string.Format(
            "{0}/reset-password?userId={1}&token={2}",
            _appSettings.ClientUrl,
            passwordResetDto.User.Id,
            Uri.EscapeDataString(passwordResetDto.ResetToken)
        );

        string emailBody = await _templateService.GetRenderedTemplateAsync(
            new ForgotPasswordTemplate(command.Username, resetLink)
        );

        await _emailService.SendEmailAsync(
            to: command.Username,
            address: passwordResetDto.User.Email,
            subject: "Password reset",
            body: emailBody,
            isHtml: true
        );
    }
}
