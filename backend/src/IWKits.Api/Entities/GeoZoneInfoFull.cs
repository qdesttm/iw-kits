using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

namespace IWKits.Api.Entities;

[BsonIgnoreExtraElements]
public sealed record GeoZoneInfoFull : GeoZoneInfo
{
	[BsonElement("coordinates")]
	public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; init; } =
		GeoJson.Point(coordinates: GeoJson.Position(0.0d, 0.0d) );
}