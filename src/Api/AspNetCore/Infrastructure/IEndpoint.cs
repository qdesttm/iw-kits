using Microsoft.AspNetCore.Builder;

namespace IWKits.Api.AspNetCore.Infrastructure;

internal interface IEndpoint
{
	RouteHandlerBuilder Map(WebApplication app);
}