using IWKits.Api.Entities;
using MongoDB.Driver;

namespace IWKits.Api.Database;

public sealed class AuthDatabaseContext : DatabaseContext
{
	public IMongoCollection<SessionInfo> Sessions => database.GetCollection<SessionInfo>("sessions");

	public IMongoCollection<UserInfo> Users => database.GetCollection<UserInfo>("users");

	public AuthDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}
}