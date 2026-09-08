using FluentValidation;

namespace IWKits.Api.Features.AuthRefresh;

public sealed class AuthRefreshRequestValidator : AbstractValidator<AuthRefreshRequest>
{
	public AuthRefreshRequestValidator()
	{
		RuleFor(x => x.RefreshToken)
			.NotEmpty().NotNull()
			.WithMessage("Refresh token is required.");
	}
}