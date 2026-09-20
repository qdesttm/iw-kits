using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Components;

/// <summary>
/// Represents a specific legal tax jurisdiction component applied to an order record.
/// </summary>
public sealed class OrderTaxJurisdictionComponent
{
	/// <summary>
	/// Gets or initializes the name of the tax jurisdiction.
	/// </summary>
	[BsonElement("name")]
	public required string Name { get; init; }

	/// <summary>
	/// Gets or initializes the type of the tax jurisdiction (e.g., "state", "county").
	/// </summary>
	[BsonElement("type")]
	public required string Type { get; init; }

	/// <summary>
	/// Gets or initializes the tax rate enforced by this jurisdiction.
	/// </summary>
	[BsonElement("rate")]
	public required decimal Rate { get; init; }
}