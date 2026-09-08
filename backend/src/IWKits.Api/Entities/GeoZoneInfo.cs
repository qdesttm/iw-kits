using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IWKits.Api.Entities;

[BsonIgnoreExtraElements]
public record GeoZoneInfo
{
	[BsonElement("state_id")]
	public string StateId { get; init; } = string.Empty;

	[BsonElement("zip_code")]
	public int ZipCode { get; init; } = int.MaxValue;

	[BsonElement("state_name")]
	public string StateName { get; init; } = string.Empty;

	[BsonElement("city_name")]
	public string CityName { get; init; } = string.Empty;

	[BsonElement("county_name")]
	public string CountyName { get; init; } = string.Empty;
}