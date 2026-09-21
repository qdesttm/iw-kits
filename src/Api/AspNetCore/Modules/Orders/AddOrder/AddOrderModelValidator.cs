using FluentValidation;

namespace IWKits.Api.AspNetCore.Modules.Orders.AddOrder;

internal sealed class AddOrderModelValidator : AbstractValidator<AddOrderModel>
{
	public AddOrderModelValidator()
	{
		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180);

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90);

		RuleFor(x => x.Subtotal)
			.GreaterThan(0);
	}
}