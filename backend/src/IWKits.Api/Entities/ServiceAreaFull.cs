using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

namespace IWKits.Api.Entities;

[BsonIgnoreExtraElements]
public sealed record ServiceAreaFull : ServiceArea
{
	[BsonElement("boundary")]
	public GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> Boundary { get; init; }
		= GeoJson.MultiPolygon(
			GeoJson.PolygonCoordinates(
				GeoJson.Geographic(0.0d, 0.0d),
				GeoJson.Geographic(1.0d, 0.0d),
				GeoJson.Geographic(0.0d, 1.0d),
				GeoJson.Geographic(0.0d, 0.0d)
			)
		);
}