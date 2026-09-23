using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Components;

/// <summary>
/// Represents the individual tax rate breakdown component of an order record.
/// </summary>
public sealed class OrderTaxBreakdownComponent
{
	/// <summary>
	/// Gets or initializes the state-level tax rate.
	/// </summary>
	[BsonElement("stateRate")]
	public required decimal StateRate { get; init; }

	/// <summary>
	/// Gets or initializes the county-level tax rate.
	/// </summary>
	[BsonElement("countyRate")]
	public required decimal CountyRate { get; init; }

	/// <summary>
	/// Gets or initializes the city-level tax rate.
	/// </summary>
	[BsonElement("cityRate")]
	public required decimal CityRate { get; init; }

	/// <summary>
	/// Gets or initializes the special local district tax rate.
	/// </summary>
	[BsonElement("specialRate")]
	public required decimal SpecialRate { get; init; }
}