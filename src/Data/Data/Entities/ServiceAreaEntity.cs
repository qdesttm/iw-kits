using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for a service area boundary defined by a geographical multi-polygon.
/// </summary>
public sealed class ServiceAreaEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the service area document.
	/// </summary>
	[BsonId]
	public required ObjectId Id { get; init; }

	/// <summary>
	/// Gets or initializes the state identifier (e.g., "NY").
	/// </summary>
	[BsonElement("stateId")]
	public required string StateId { get; init; }

	/// <summary>
	/// Gets or initializes the GeoJSON MultiPolygon representing the geographical boundaries of the service area.
	/// </summary>
	[BsonElement("boundary")]
	public required GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> Boundary { get; init; }
}
