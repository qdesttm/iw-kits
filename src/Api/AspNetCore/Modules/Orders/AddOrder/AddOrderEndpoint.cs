using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IWKits.Api.AspNetCore.Infrastructure;
using IWKits.Api.AspNetCore.Versioning;
using IWKits.Core.Application.Modules.Orders.Models;
using IWKits.Core.Common.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IWKits.Api.AspNetCore.Modules.Orders.AddOrder;

internal sealed class AddOrderEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapPost("/v{version:apiVersion}/orders", Handle)
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Orders")
			.WithSummary("Create a new order")
			.WithDescription("Evaluates sales tax based on coordinates and stores a finalized order record.")
			.Produces<AddOrderResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden);
	}

	private static async Task<Ok<AddOrderResponse>> Handle(
		[AsParameters] AddOrderModel model,
		IValidator<AddOrderModel> validator,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(model, cancellationToken);

		var request = model.ToAddOrderRequest();
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}