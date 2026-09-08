using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IWKits.Api.Services;
using System.Threading;
using FluentValidation;

namespace IWKits.Api.Features.AuthRefresh;

public static class AuthRefreshEndpoint
{
	public const string Endpoint = "/auth/refresh";

	public static void MapAuthRefreshEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AuthRefreshHandlerAsync)
			.Produces<AuthRefreshRespond>(200)
			.Produces(403)
			.WithName("AuthRefresh");
	}

	private static async Task<IResult> AuthRefreshHandlerAsync
	(
		[FromServices] IValidator<AuthRefreshRequest> validator,
		[FromServices] ISessionService sessionService,
		[FromBody] AuthRefreshRequest request,
		CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		var errorMessage = validationResult.JoinErrorsOrEmpty();

		if ( !string.IsNullOrEmpty(errorMessage) )
		{
			var respond = new AuthRefreshRespond(null, null, errorMessage);
			return Results.BadRequest(respond);
		}

		var refreshResult = await sessionService.RefreshSessionAsync(request.RefreshToken, ct);

		if ( refreshResult.HasError )
		{
			var respond = new AuthRefreshRespond(null, null, refreshResult.ErrorMessage);
			return Results.Json(respond, statusCode: 403);
		}

		return Results.Ok<AuthRefreshRespond>(new()
		{
			AccessToken = refreshResult.AccessToken,
			RefreshToken = refreshResult.Session.RefreshToken
		});
	}
}