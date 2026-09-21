using Microsoft.AspNetCore.Builder;

namespace IWKits.Api.AspNetCore.Hosting;

internal interface IEndpoint
{
	RouteHandlerBuilder Map(WebApplication app);
}