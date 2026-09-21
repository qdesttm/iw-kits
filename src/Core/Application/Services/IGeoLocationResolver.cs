using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal interface IGeoLocationResolver
{
	Task<Location?> FindLocationAsync(
		NetTopologySuite.Geometries.Point coordinates,
		string state,
		CancellationToken cancellationToken);
}
