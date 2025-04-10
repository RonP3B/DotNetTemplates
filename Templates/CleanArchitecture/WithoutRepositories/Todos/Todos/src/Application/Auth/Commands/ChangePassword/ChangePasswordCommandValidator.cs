using FluentValidation;

namespace Todos.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(v => v.NewPassword).NotEmpty();

        RuleFor(v => v.CurrentPassword).NotEmpty();

        RuleFor(v => v.UserId).NotEmpty();
    }
}
