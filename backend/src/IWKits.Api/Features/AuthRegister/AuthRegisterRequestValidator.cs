using FluentValidation;

namespace IWKits.Api.Features.AuthRegister;

public sealed class AuthRegisterRequestValidator : AbstractValidator<AuthRegisterRequest>
{
	public AuthRegisterRequestValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty().NotNull()
			.WithMessage("Username is required.");

		RuleFor(x => x.Password)
			.NotEmpty().NotNull()
			.WithMessage("Password is required.");
	}
}