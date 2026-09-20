using MongoDB.Driver.GeoJsonObjectModel;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Mappers;

internal static partial class CoordinatesMapper
{
	public static GeoJson2DGeographicCoordinates ToGeoJson2DGeographicCoordinates(this Coordinate coordinate)
		=> new(coordinate.X, coordinate.Y);

	public static Coordinate ToCoordinate(this GeoJson2DGeographicCoordinates coordinates)
		=> new(coordinates.Longitude, coordinates.Latitude);
}