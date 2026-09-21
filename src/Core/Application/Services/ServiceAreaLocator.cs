using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using MongoDB.Driver;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Services;

internal sealed class ServiceAreaLocator(
	MongoDbContext dbContext) :
	IServiceAreaLocator
{
	private List<ServiceArea> _serviceAreas = [];

	public async Task<ServiceArea?> FindAreaAsync(Point coordinates)
		=> _serviceAreas.FirstOrDefault(a => a.Boundary.Contains(coordinates));

	public async Task RefreshAreasAsync(CancellationToken cancellationToken)
	{
		var entities = await dbContext.ServiceAreas
			.Find(_ => true)
			.ToListAsync(cancellationToken);

		_serviceAreas = [.. entities
			.Select(ServiceAreaMapper.ToServiceArea)];
	}
}