using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace IWKits.Api.AspNetCore.Hosting;

internal static class WebApplicationExtensions
{
	public static WebApplication MapEndpoints(this WebApplication app)
	{
		var assembly = Assembly.GetCallingAssembly();

		var endpointTypes = assembly.GetTypes()
			.Where(t => typeof(IEndpoint).IsAssignableFrom(t)
						&& !t.IsInterface 
						&& !t.IsAbstract);

		foreach (var type in endpointTypes)
		{
			var endpoint = (IEndpoint) Activator.CreateInstance(type)!;
			endpoint.Map(app);
		}

		return app;
	}
}