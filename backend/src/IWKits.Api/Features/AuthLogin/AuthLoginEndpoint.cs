using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IWKits.Api.Database;
using IWKits.Api.Services;
using IWKits.Api.Entities;
using IWKits.Api.Settings;
using System.Threading;
using FluentValidation;
using MongoDB.Driver;

namespace IWKits.Api.Features.AuthLogin;

public static class AuthLoginEndpoint
{
	public const string Endpoint = "/auth/login";

	public static void MapAuthLoginEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AuthLoginHandlerAsync)
			.Produces<AuthLoginRespond>(200)
			.Produces(401)
			.WithName("AuthLogin");
	}

	private static async Task<IResult> AuthLoginHandlerAsync
	(
		[FromServices] IOptions<SecuritySettings> securitySettings,
		[FromServices] IValidator<AuthLoginRequest> validator,
		[FromServices] AuthDatabaseContext authDatabase,
		[FromServices] ISecurityService securityService,
		[FromServices] ISessionService sessionService,
		[FromBody] AuthLoginRequest request,
		HttpContext httpContext, CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		var errorMessage = validationResult.JoinErrorsOrEmpty();

		if ( !string.IsNullOrEmpty(errorMessage) )
		{
			var respond = new AuthLoginRespond(null, null, null, errorMessage);
			return Results.BadRequest(respond);
		}

		var filter = Builders<UserInfo>.Filter.Eq(x => x.Username, request.Username);
		var userInfo = await authDatabase.Users.Find(filter).FirstOrDefaultAsync(ct);

		if ( userInfo is null || !securityService.VerifyPassword(userInfo.Password, request.Password) )
		{
			var respond = new AuthLoginRespond(null, null, null, "Invalid credentials");
			return Results.Json(respond, statusCode: 401);
		}

		var createResult = await sessionService.CreateSessionAsync(userInfo, ct);

		return Results.Ok<AuthLoginRespond>(new
		(
			RefreshToken: createResult.Session.RefreshToken,
			AccessToken : createResult.AccessToken,
			User        : userInfo
		));
	}
}