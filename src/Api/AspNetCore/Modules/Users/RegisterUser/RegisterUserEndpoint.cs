using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IWKits.Api.AspNetCore.Infrastructure;
using IWKits.Api.AspNetCore.Versioning;
using IWKits.Core.Common.Messages;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IWKits.Api.AspNetCore.Modules.Users.RegisterUser;

internal sealed class RegisterUserEndpoint : IEndpoint
{
	public RouteHandlerBuilder Map(WebApplication app)
	{
		return app.MapPost("/v{version:apiVersion}/users", Handle)
			.WithApiVersionSet(ApiVersionSetFactory.GetOrCreate(app))
			.MapToApiVersion(1)
			.WithTags("Users")
			.WithSummary("Register a new user account")
			.WithDescription("Creates a new secure user identity in the system with default member roles.")
			.Produces(StatusCodes.Status200OK)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
	}

	private static async Task<Ok> Handle(
		[AsParameters] RegisterUserModel model,
		IValidator<RegisterUserModel> validator,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(model, cancellationToken);

		var request = model.ToRegisterUserRequest();
		await mediator.Send(request, cancellationToken);

		return TypedResults.Ok();
	}
}