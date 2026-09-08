using FluentValidation;

namespace IWKits.Api.Features.AuthLogin;

public sealed class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
{
	public AuthLoginRequestValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty().NotNull()
			.WithMessage("Username is required.");

		RuleFor(x => x.Password)
			.NotEmpty().NotNull()
			.WithMessage("Password is required.");
	}
}