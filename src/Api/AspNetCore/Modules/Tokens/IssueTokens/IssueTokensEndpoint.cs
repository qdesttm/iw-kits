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

namespace IWKits.Api.AspNetCore.Modules.Tokens.IssueTokens;

internal sealed class IssueTokensEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapPost("/v{version:apiVersion}/tokens", Handle)
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Tokens")
			.WithSummary("Issue security tokens")
			.WithDescription("Verifies user credentials and issues a new pair of access and refresh tokens.")
			.Produces<TokensResponse>(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
	}

	private static async Task<Ok<TokensResponse>> Handle(
		[AsParameters] IssueTokensModel model,
		IValidator<IssueTokensModel> validator,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(model, cancellationToken);

		var request = model.ToIssueTokensRequest();
		var response = await mediator.Send(request, cancellationToken);

		return TypedResults.Ok(response);
	}
}