using FluentValidation;

namespace Todos.Application.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty();

        RuleFor(v => v.Password).NotEmpty();
    }
}
