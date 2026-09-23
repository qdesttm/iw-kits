using IWKits.Core.Mongodb.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IWKits.Core.Data.DependencyInjection;

/// <summary>
/// Extends <see cref="IServiceCollection"/> with database services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds database services to the specified <see cref="IServiceCollection"/> using explicit connection parameters.
	/// </summary>
	/// <param name="services">The service collection to add the application services to.</param>
	/// <returns>The original <see cref="IServiceCollection"/> instance for chaining.</returns>
	public static IServiceCollection AddDbContext(this IServiceCollection services)
	{
		services.AddSingleton<IMongoClient>(sp =>
		{
			var options = sp.GetRequiredService<IOptions<MongodbOptions>>();

			return new MongoClient(options.Value.ConnectionString);
		});

		services.AddSingleton(sp =>
		{
			var options = sp.GetRequiredService<IOptions<MongodbOptions>>();
			var client = sp.GetRequiredService<IMongoClient>();

			return client.GetDatabase(options.Value.DatabaseName);
		});

		services.AddSingleton<MongoDbContext>();

		return services;
	}
}