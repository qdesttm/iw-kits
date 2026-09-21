using FluentValidation;

namespace IWKits.Api.AspNetCore.Modules.Users.RegisterUser;

internal sealed class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
	public RegisterUserModelValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty()
			.MaximumLength(32);

		RuleFor(x => x.Password)
			.NotEmpty()
			.MaximumLength(64);
	}
}