using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Common.Models;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Services;

internal interface IServiceAreaLocator
{
	Task<ServiceArea?> FindAreaAsync(Point coordinates);

	Task RefreshAreasAsync(CancellationToken cancellationToken);
}
