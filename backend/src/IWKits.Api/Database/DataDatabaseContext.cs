using IWKits.Api.Entities;
using MongoDB.Driver;

namespace IWKits.Api.Database;

public sealed class DataDatabaseContext : DatabaseContext
{
	public IMongoCollection<OrderInfo> Orders => database.GetCollection<OrderInfo>("orders");

	public DataDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}
}