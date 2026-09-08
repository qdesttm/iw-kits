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
using System;

namespace IWKits.Api.Features.AuthRegister;

public static class AuthRegisterEndpoint
{
	public const string Endpoint = "/auth/register";

	public static void MapAuthRegisterEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AuthRegisterHandlerAsync)
			.Produces<AuthRegisterRespond>(200)
			.Produces(400)
			.WithName("AuthRegister");
	}

	private static async Task<IResult> AuthRegisterHandlerAsync
	(
		[FromServices] IOptions<SecuritySettings> securitySettings,
		[FromServices] IValidator<AuthRegisterRequest> validator,
		[FromServices] AuthDatabaseContext authDatabase,
		[FromServices] ISecurityService securityService,
		[FromServices] ISessionService sessionService,
		[FromBody] AuthRegisterRequest request,
		CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		var errorMessage = validationResult.JoinErrorsOrEmpty();

		if ( !string.IsNullOrEmpty(errorMessage) )
		{
			var respond = new AuthRegisterRespond(null, null, null, errorMessage);
			return Results.BadRequest(respond);
		}

		var filter = Builders<UserInfo>.Filter.Eq(x => x.Username, request.Username);
		var userExists = await authDatabase.Users.Find(filter).AnyAsync(ct);

		if ( userExists )
		{
			var respond = new AuthRegisterRespond(null, null, null, "Username already taken");
			return Results.Json(respond, statusCode: 400);
		}

		var hashpass = securityService.HashPassword(request.Password);

		var userInfo = new UserInfo()
		{
			Id       = Guid.NewGuid(),
			Username = request.Username,
			Password = hashpass,
			Role     = "user"
		};

		await authDatabase.Users.InsertOneAsync(userInfo, null, ct);

		var createResult = await sessionService.CreateSessionAsync(userInfo, ct);

		return Results.Ok<AuthRegisterRespond>(new
		(
			RefreshToken: createResult.Session.RefreshToken,
			AccessToken : createResult.AccessToken,
			User        : userInfo
		));
	}
}