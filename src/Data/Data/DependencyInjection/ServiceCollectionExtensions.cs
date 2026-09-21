using Microsoft.Extensions.DependencyInjection;
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
	/// <param name="connectionString">The MongoDB server connection string.</param>
	/// <param name="databaseName">The target MongoDB database name.</param>
	/// <returns>The original <see cref="IServiceCollection"/> instance for chaining.</returns>
	public static IServiceCollection AddDatabase(
		this IServiceCollection services, string connectionString, string databaseName)
	{
		var mongoClient = new MongoClient(connectionString);

		services.AddSingleton<IMongoClient>(mongoClient);

		services.AddSingleton(sp =>
		{
			var client = sp.GetRequiredService<IMongoClient>();
			return client.GetDatabase(databaseName);
		});

		services.AddSingleton<MongoDbContext>();

		return services;
	}
}