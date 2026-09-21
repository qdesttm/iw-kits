using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for a geographical and postal location.
/// </summary>
public sealed class LocationEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the location document.
	/// </summary>
	[BsonId]
	public required ObjectId Id { get; init; }

	/// <summary>
	/// Gets or initializes the state identifier.
	/// </summary>
	[BsonElement("stateId")]
	public required string StateId { get; init; }

	/// <summary>
	/// Gets or initializes the numerical postal or ZIP code.
	/// </summary>
	[BsonElement("zipCode")]
	public required int ZipCode { get; init; }

	/// <summary>
	/// Gets or initializes the full name of the state.
	/// </summary>
	[BsonElement("stateName")]
	public required string StateName { get; init; }

	/// <summary>
	/// Gets or initializes the name of the city.
	/// </summary>
	[BsonElement("cityName")]
	public required string CityName { get; init; }

	/// <summary>
	/// Gets or initializes the name of the county.
	/// </summary>
	[BsonElement("countyName")]
	public required string CountyName { get; init; }

	/// <summary>
	/// Gets or initializes the GeoJSON 2D point containing longitude and latitude coordinates.
	/// </summary>
	[BsonElement("coordinates")]
	public required GeoJsonPoint<GeoJson2DGeographicCoordinates> Coordinates { get; init; }
}