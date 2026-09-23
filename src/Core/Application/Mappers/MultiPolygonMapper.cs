using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.GeoJsonObjectModel;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Mappers;

internal static class MultiPolygonMapper
{
	public static MultiPolygon ToMultiPolygon(this GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> geoJsonMultiPolygon)
	{
		var factory = GeometryFactory.Floating;
		var listPolygons = new List<Polygon>();

		foreach (var geoJsonPolygon in geoJsonMultiPolygon.Coordinates.Polygons)
		{
			var shellCoords = geoJsonPolygon.Exterior.Positions
				.Select(CoordinatesMapper.ToCoordinates)
				.ToArray();

			var holes = geoJsonPolygon.Holes
				.Select(h => factory.CreateLinearRing(
						[.. h.Positions.Select(CoordinatesMapper.ToCoordinates)]))
				.ToArray();

			var shell = factory.CreateLinearRing(shellCoords);
			var polygon = factory.CreatePolygon(shell, holes);

			listPolygons.Add(polygon);
		}

		return factory.CreateMultiPolygon([..listPolygons]);
	}
}