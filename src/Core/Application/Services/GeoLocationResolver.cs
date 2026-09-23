using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MongoDB.Driver;

namespace IWKits.Core.Application.Services;

internal sealed class GeoLocationResolver(
	MongoDbContext dbContext) :
	IGeoLocationResolver
{
	public async Task<Location?> FindLocationAsync(
		NetTopologySuite.Geometries.Point coordinates,
		string state,
		CancellationToken cancellationToken)
	{
		var filterBuilder = Builders<LocationEntity>.Filter;

		var filter = filterBuilder.And(
			filterBuilder.NearSphere(x => x.Coordinates, coordinates.X, coordinates.Y),
			filterBuilder.Eq(x => x.StateId, state)
		);

		var entity = await dbContext.Locations
			.Find(filter)
			.FirstOrDefaultAsync(cancellationToken);

		return entity?.ToLocation();
	}
}