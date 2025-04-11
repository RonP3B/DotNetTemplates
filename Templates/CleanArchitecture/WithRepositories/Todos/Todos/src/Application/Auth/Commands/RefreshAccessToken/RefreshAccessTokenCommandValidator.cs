using FluentValidation;

namespace Todos.Application.Auth.Commands.RefreshAccessToken;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(v => v.RefreshToken).NotEmpty().WithMessage("Missing refresh token");
    }
}
