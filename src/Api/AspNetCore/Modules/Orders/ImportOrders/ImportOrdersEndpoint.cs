using System.Threading;
using System.Threading.Tasks;
using IWKits.Api.AspNetCore.Hosting;
using IWKits.Api.AspNetCore.Versioning;
using IWKits.Core.Application.Modules.Orders.Models;
using IWKits.Core.Application.Modules.Orders.Models.Requests;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IWKits.Api.AspNetCore.Modules.Orders.ImportOrders;

internal sealed class ImportOrdersEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapPost("/v{version:apiVersion}/orders/import", Handle)
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Orders")
			.WithSummary("Bulk import orders from CSV")
			.WithDescription("Processes a bulk CSV file containing raw order spatial data in parallel and saves them.")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<ImportOrdersResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden);
	}

	private static async Task<Ok<ImportOrdersResponse>> Handle(
		[FromForm] IFormFile file,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		if (file is null || file.Length == 0)
			throw new InvalidFileException();

		using var stream = file.OpenReadStream();

		var request = new ImportOrdersRequest(stream);
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}