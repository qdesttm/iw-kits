using FluentValidation;

namespace IWKits.Api.AspNetCore.Modules.Tokens.IssueTokens;

internal sealed class IssueTokensModelValidator : AbstractValidator<IssueTokensModel>
{
	public IssueTokensModelValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty()
			.MaximumLength(32);

		RuleFor(x => x.Password)
			.NotEmpty()
			.MaximumLength(64);
	}
}