using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;

namespace IWKits.Api.AspNetCore.Versioning;

internal static class ApiVersionSetFactory
{
	private static ApiVersionSet? _apiVersionSet;

	public static ApiVersionSet GetOrCreate(WebApplication app)
		=> _apiVersionSet ??= app.NewApiVersionSet().HasApiVersion(1).Build();
}