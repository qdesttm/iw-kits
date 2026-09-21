using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Api.AspNetCore.Hosting;
using IWKits.Api.AspNetCore.Versioning;
using IWKits.Core.Application.Modules.Users.Models;
using IWKits.Core.Application.Modules.Users.Models.Requests;
using IWKits.Core.Common.Messages;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IWKits.Api.AspNetCore.Modules.Users.GetUserProfile;

internal sealed class GetUserProfileEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapGet("/v{version:apiVersion}/users/profile", Handle)
			.RequireAuthorization()
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Users")
			.WithSummary("Get current user profile")
			.WithDescription("Retrieves the secure profile details for the currently authenticated user session.")
			.Produces<UserProfileResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized);
	}

	private static async Task<Ok<UserProfileResponse>> Handle(
		ClaimsPrincipal user,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		var rawUserId = user.FindFirstValue(ClaimTypes.NameIdentifier)
			?? throw new InvalidOperationException("User identifier claim is missing in the current security context.");

		var userId = Guid.Parse(rawUserId);

		var request = new UserProfileRequest(userId);
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}