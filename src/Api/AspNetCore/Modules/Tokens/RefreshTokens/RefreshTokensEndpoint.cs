using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IWKits.Api.AspNetCore.Infrastructure;
using IWKits.Api.AspNetCore.Versioning;
using IWKits.Core.Application.Modules.Tokens.Models;
using IWKits.Core.Common.Messages;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IWKits.Api.AspNetCore.Modules.Tokens.RefreshTokens;

internal sealed class RefreshTokensEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapPost("/v{version:apiVersion}/tokens/refresh", Handle)
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Tokens")
			.WithSummary("Refresh security tokens")
			.WithDescription("Rotates an active refresh token to issue a new pair of access and refresh tokens.")
			.Produces<TokensResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
	}

	private static async Task<Ok<TokensResponse>> Handle(
		[AsParameters] RefreshTokensModel model,
		IValidator<RefreshTokensModel> validator,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(model, cancellationToken);

		var request = model.ToRefreshTokensRequest();
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}