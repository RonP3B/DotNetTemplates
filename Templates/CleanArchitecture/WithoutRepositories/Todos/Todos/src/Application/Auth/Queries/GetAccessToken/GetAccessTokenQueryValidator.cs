using FluentValidation;

namespace Todos.Application.Auth.Queries.GetAccessToken;

public class GetAccessTokenQueryValidator : AbstractValidator<GetAccessTokenQuery>
{
    public GetAccessTokenQueryValidator()
    {
        RuleFor(v => v.RefreshToken).NotEmpty().WithMessage("Missing refresh token");
    }
}
