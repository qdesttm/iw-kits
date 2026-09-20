using IWKits.Core.Data.Entities;
using MongoDB.Driver;

namespace IWKits.Core.Data;

/// <summary>
/// Represents the MongoDB database context.
/// </summary>
public sealed class MongoDbContext
{
	private readonly IMongoDatabase _db;

	/// <summary>
	/// Initializes a new <see cref="MongoDbContext"/> instance with the specified Mongo client and database name.
	/// </summary>
	/// <param name="client">The MongoDB client instance.</param>
	/// <param name="databaseName">The name of the target database (e.g., "kits").</param>
	public MongoDbContext(IMongoClient client, string databaseName)
	{
		_db = client.GetDatabase(databaseName);
	}

	/// <summary>
	/// The data collection to be used to operate with <see cref="SessionEntity"/> entities.
	/// </summary>
	public IMongoCollection<SessionEntity> Sessions => _db.GetCollection<SessionEntity>("sessions");

	/// <summary>
	/// The data collection to be used to operate with <see cref="UserEntity"/> entities.
	/// </summary>
	public IMongoCollection<UserEntity> Users => _db.GetCollection<UserEntity>("users");

	/// <summary>
	/// The data collection to be used to operate with <see cref="OrderRecordEntity"/> entities.
	/// </summary>
	public IMongoCollection<OrderRecordEntity> OrderRecords => _db.GetCollection<OrderRecordEntity>("order_records");

	/// <summary>
	/// The data collection to be used to operate with <see cref="LocationEntity"/> entities.
	/// </summary>
	public IMongoCollection<LocationEntity> Locations => _db.GetCollection<LocationEntity>("locations");

	/// <summary>
	/// The data collection to be used to operate with <see cref="ServiceAreaEntity"/> entities.
	/// </summary>
	public IMongoCollection<ServiceAreaEntity> ServiceAreas => _db.GetCollection<ServiceAreaEntity>("service_areas");

	/// <summary>
	/// The data collection to be used to operate with <see cref="TaxRateEntity"/> entities.
	/// </summary>
	public IMongoCollection<TaxRateEntity> TaxRates => _db.GetCollection<TaxRateEntity>("tax_rates");
}