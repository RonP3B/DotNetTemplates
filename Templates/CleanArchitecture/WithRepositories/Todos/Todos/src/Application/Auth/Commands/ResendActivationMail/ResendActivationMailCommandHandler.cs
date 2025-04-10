using Todos.Application.Auth.DTOs;
using Todos.Application.Users.TemplateModels;

namespace Todos.Application.Auth.Commands.ResendActivationMail;

public class ResendActivationMailCommandHandler(
    IAuthService authService,
    IEmailService emailService,
    IAppSettings appSettings,
    ITemplateService templateService
) : IRequestHandler<ResendActivationMailCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly IEmailService _emailService = emailService;
    private readonly IAppSettings _appSettings = appSettings;
    private readonly ITemplateService _templateService = templateService;

    public async Task Handle(
        ResendActivationMailCommand command,
        CancellationToken cancellationToken
    )
    {
        ApplicationResult<UserActivationDto> result = await _authService.GetActivationTokenAsync(
            command.Username
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }

        UserActivationDto userActivationDto = Guard.Against.Null(result.Value);

        string activationLink = string.Format(
            "{0}/api/v1/auth/account-activation?username={1}&token={2}",
            _appSettings.BaseUrl,
            userActivationDto.User.UserName,
            Uri.EscapeDataString(userActivationDto.ActivationToken)
        );

        string emailBody = await _templateService.GetRenderedTemplateAsync(
            new AccountActivationTemplate(userActivationDto.User.UserName, activationLink)
        );

        await _emailService.SendEmailAsync(
            to: userActivationDto.User.UserName,
            address: userActivationDto.User.Email,
            subject: "Activate Your Account",
            body: emailBody,
            isHtml: true
        );
    }
}
