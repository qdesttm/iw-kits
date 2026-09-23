using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for tax rates associated with a specific postal code and state id.
/// </summary>
public sealed class TaxRateEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the tax rate document.
	/// </summary>
	[BsonId]
	public required ObjectId Id { get; init; }

	/// <summary>
	/// Gets or initializes the numerical postal or ZIP code.
	/// </summary>
	[BsonElement("zipCode")]
	public required int ZipCode { get; init; }

	/// <summary>
	/// Gets or initializes the state identifier (e.g., "NY").
	/// </summary>
	[BsonElement("stateId")]
	public required string StateId { get; init; }

	/// <summary>
	/// Gets or initializes the sales tax rate for the state.
	/// </summary>
	[BsonElement("stateRate")]
	public required decimal StateRate { get; init; }

	/// <summary>
	/// Gets or initializes the total estimated combined tax rate (state + county + city + special).
	/// </summary>
	[BsonElement("estimatedCombinedRate")]
	public required decimal EstimatedCombinedRate { get; init; }

	/// <summary>
	/// Gets or initializes the estimated tax rate for the county.
	/// </summary>
	[BsonElement("estimatedCountyRate")]
	public required decimal EstimatedCountyRate { get; init; }

	/// <summary>
	/// Gets or initializes the estimated tax rate for the city.
	/// </summary>
	[BsonElement("estimatedCityRate")]
	public required decimal EstimatedCityRate { get; init; }

	/// <summary>
	/// Gets or initializes the estimated tax rate for special local districts or transit authorities.
	/// </summary>
	[BsonElement("estimatedSpecialRate")]
	public required decimal EstimatedSpecialRate { get; init; }
}
