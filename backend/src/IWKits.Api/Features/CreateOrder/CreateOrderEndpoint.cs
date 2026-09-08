using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Services;
using IWKits.Api.Database;
using System.Threading;
using FluentValidation;
using System;

namespace IWKits.Api.Features.CreateOrder;

public static class CreateOrderEndpoint
{
	public const string Endpoint = "/orders";

	public static void MapCreateOrderEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, CreateOrderHandlerAsync)
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.Produces<CreateOrderRespond>(200)
			.WithName("CreateOrder");
	}

	private static async Task<IResult> CreateOrderHandlerAsync
	(
		[FromServices] IValidator<CreateOrderRequest> validator,
		[FromServices] IOrderProcessService orderProcess,
		[FromServices] DataDatabaseContext dataDatabase,
		[FromBody] CreateOrderRequest request,
		HttpContext httpContext, CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		var errorMessage = validationResult.JoinErrorsOrEmpty();

		if ( !string.IsNullOrEmpty(errorMessage) )
		{
			var respond = new CreateOrderRespond(null, errorMessage);
			return Results.BadRequest(respond);
		}

		var rawOrderInfo = new RawOrderInfo()
		{
			Id = 0,

			Longitude = request.Longitude,
			Latitude  = request.Latitude,
			Subtotal  = request.Subtotal,
			Timestamp = DateTime.UtcNow
		};

		var processResult = await orderProcess.ProcessAsync(rawOrderInfo);

		if ( processResult.HasError )
		{
			var respond = new CreateOrderRespond(null, processResult.ErrorMessage);
			return Results.Ok(respond);
		}
		else
		{
			OrderInfo orderInfo = processResult.OrderInfo;

			await dataDatabase.Orders.InsertOneAsync(orderInfo, null, ct);

			var respond = new CreateOrderRespond(orderInfo, null);
			return Results.Created($"{Endpoint}/{orderInfo.Id}", respond);
		}
	}
}