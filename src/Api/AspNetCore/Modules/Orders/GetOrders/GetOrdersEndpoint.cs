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

namespace IWKits.Api.AspNetCore.Modules.Orders.GetOrders;

internal sealed class ListOrdersEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapGet("/v{version:apiVersion}/orders", Handle)
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Orders")
			.WithSummary("Get order records history")
			.WithDescription("Retrieves a filtered, sorted, and paginated list of all finalized order records.")
			.Produces<ListOrdersResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden);
	}

	private static async Task<Ok<ListOrdersResponse>> Handle(
		[AsParameters] GetOrdersModel model,
		IValidator<GetOrdersModel> validator,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(model, cancellationToken);

		var request = model.ToListOrdersRequest();
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}