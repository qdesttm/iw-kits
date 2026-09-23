using IWKits.Core.Data.Entities;
using MongoDB.Driver;

namespace IWKits.Core.Data;

/// <summary>
/// Represents the MongoDB database context.
/// </summary>
public sealed class MongoDbContext(IMongoDatabase db)
{
	/// <summary>
	/// The data collection to be used to operate with <see cref="UserSessionEntity"/> entities.
	/// </summary>
	public IMongoCollection<UserSessionEntity> UserSessions => db.GetCollection<UserSessionEntity>("user_sessions");

	/// <summary>
	/// The data collection to be used to operate with <see cref="UserEntity"/> entities.
	/// </summary>
	public IMongoCollection<UserEntity> Users => db.GetCollection<UserEntity>("users");

	/// <summary>
	/// The data collection to be used to operate with <see cref="OrderRecordEntity"/> entities.
	/// </summary>
	public IMongoCollection<OrderRecordEntity> OrderRecords => db.GetCollection<OrderRecordEntity>("order_records");

	/// <summary>
	/// The data collection to be used to operate with <see cref="LocationEntity"/> entities.
	/// </summary>
	public IMongoCollection<LocationEntity> Locations => db.GetCollection<LocationEntity>("locations");

	/// <summary>
	/// The data collection to be used to operate with <see cref="ServiceAreaEntity"/> entities.
	/// </summary>
	public IMongoCollection<ServiceAreaEntity> ServiceAreas => db.GetCollection<ServiceAreaEntity>("service_areas");

	/// <summary>
	/// The data collection to be used to operate with <see cref="TaxRateEntity"/> entities.
	/// </summary>
	public IMongoCollection<TaxRateEntity> TaxRates => db.GetCollection<TaxRateEntity>("tax_rates");
}