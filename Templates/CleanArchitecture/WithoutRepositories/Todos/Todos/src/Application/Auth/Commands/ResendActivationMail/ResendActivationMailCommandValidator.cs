using FluentValidation;

namespace Todos.Application.Auth.Commands.ResendActivationMail;

public class ResendActivationMailCommandValidator : AbstractValidator<ResendActivationMailCommand>
{
    public ResendActivationMailCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty();
    }
}
