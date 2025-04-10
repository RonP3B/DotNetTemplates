using FluentValidation;

namespace Todos.Application.Auth.Commands.ActivateAccount;

public class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
{
    public ActivateAccountCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty();

        RuleFor(v => v.ActivationToken).NotEmpty();
    }
}
