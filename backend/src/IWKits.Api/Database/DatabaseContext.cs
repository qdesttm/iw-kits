using MongoDB.Driver;

namespace IWKits.Api.Database;

public abstract class DatabaseContext
{
	protected readonly IMongoDatabase database;

	public DatabaseContext(IMongoClient client, string databaseName)
	{
		database = client.GetDatabase(databaseName);
	}
}