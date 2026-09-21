using MongoDB.Driver.GeoJsonObjectModel;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Mappers;

internal static partial class CoordinatesMapper
{
	public static GeoJson2DGeographicCoordinates ToGeoJson2DGeographicCoordinates(this Coordinate coordinate)
		=> new(coordinate.X, coordinate.Y);

	public static Coordinate ToCoordinates(this GeoJson2DGeographicCoordinates coordinates)
		=> new(coordinates.Longitude, coordinates.Latitude);

	public static Coordinate ToCoordinates(this GeoJsonPoint<GeoJson2DGeographicCoordinates> point)
		=> point.Coordinates.ToCoordinates();
}