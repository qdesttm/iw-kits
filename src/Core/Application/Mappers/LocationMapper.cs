using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class LocationMapper
{
	[MapPropertyFromSource(nameof(Location.Coordinates), Use = nameof(MapCoordinates))]
	public static partial Location ToLocation(this LocationEntity entity);

	private static NetTopologySuite.Geometries.Coordinate MapCoordinates(this LocationEntity entity)
		=> entity.Coordinates.ToCoordinates();
}