using IWKits.Api.Entities;
using MongoDB.Driver;

namespace IWKits.Api.Database;

public sealed class CoreDatabaseContext : DatabaseContext
{
	public IMongoCollection<TaxRateInfoFull> TaxRates => database.GetCollection<TaxRateInfoFull>("taxrates");

	public IMongoCollection<GeoZoneInfoFull> GeoZones => database.GetCollection<GeoZoneInfoFull>("geozones");

	public IMongoCollection<ServiceAreaFull> SerAreas => database.GetCollection<ServiceAreaFull>("serareas");

	public CoreDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}
}