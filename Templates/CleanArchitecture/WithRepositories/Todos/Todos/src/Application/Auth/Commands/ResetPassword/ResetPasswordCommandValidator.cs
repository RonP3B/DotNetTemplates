using FluentValidation;

namespace Todos.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(v => v.NewPassword).NotEmpty();

        RuleFor(v => v.ResetToken).NotEmpty();

        RuleFor(v => v.UserId).NotEmpty();
    }
}
