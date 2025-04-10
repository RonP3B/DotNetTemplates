using Todos.Application.Users.DTOs;
using Todos.Application.Users.TemplateModels;

namespace Todos.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    IEmailService emailService,
    IAppSettings appSettings,
    ITemplateService templateService
) : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;
    private readonly IEmailService _emailService = emailService;
    private readonly IAppSettings _appSettings = appSettings;
    private readonly ITemplateService _templateService = templateService;

    public async Task<UserDto> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        ApplicationResult<CreatedUserDto> result = await _applicationUserManager.CreateUserAsync(
            command
        );

        if (!result.Succeeded)
        {
            throw new ValidationException(result.ToValidationFailures());
        }

        CreatedUserDto createdUserDto = Guard.Against.Null(result.Value);

        string activationLink = string.Format(
            "{0}/api/v1/auth/account-activation?username={1}&token={2}",
            _appSettings.BaseUrl,
            createdUserDto.User.UserName,
            Uri.EscapeDataString(createdUserDto.ActivationToken)
        );

        string emailBody = await _templateService.GetRenderedTemplateAsync(
            new AccountActivationTemplate(createdUserDto.User.UserName, activationLink)
        );

        await _emailService.SendEmailAsync(
            to: createdUserDto.User.UserName,
            address: createdUserDto.User.Email,
            subject: "Activate Your Account",
            body: emailBody,
            isHtml: true
        );

        return createdUserDto.User;
    }
}
