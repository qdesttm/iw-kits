using System;
using FluentValidation;

namespace IWKits.Api.AspNetCore.Modules.Orders.GetOrders;

internal sealed class GetOrdersModelValidator : AbstractValidator<GetOrdersModel>
{
	public GetOrdersModelValidator()
	{
		RuleFor(x => x.Page)
			.GreaterThan(0)
			.When(x => x.Page.HasValue);

		RuleFor(x => x.Size)
			.GreaterThan(0)
			.When(x => x.Size.HasValue);

		RuleFor(x => x.MinTotalAmount)
			.GreaterThanOrEqualTo(0)
			.When(x => x.MinTotalAmount.HasValue);

		RuleFor(x => x.MaxTotalAmount)
			.GreaterThanOrEqualTo(0)
			.GreaterThanOrEqualTo(x => x.MinTotalAmount ?? 0)
			.When(x => x.MaxTotalAmount.HasValue);

		RuleFor(x => x.Before)
			.GreaterThanOrEqualTo(x => x.After ?? DateTime.MinValue)
			.When(x => x.Before.HasValue && x.After.HasValue);
	}
}