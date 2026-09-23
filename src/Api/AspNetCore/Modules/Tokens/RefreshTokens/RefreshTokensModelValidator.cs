using FluentValidation;

namespace IWKits.Api.AspNetCore.Modules.Tokens.RefreshTokens;

internal sealed class RefreshTokensModelValidator : AbstractValidator<RefreshTokensModel>
{
	public RefreshTokensModelValidator()
	{
		RuleFor(x => x.RefreshToken)
			.NotEmpty()
			.MaximumLength(256);
	}
}