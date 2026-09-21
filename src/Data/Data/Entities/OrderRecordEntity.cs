using System;
using System.Collections.Generic;
using IWKits.Core.Data.Components;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for a finalized order financial and tax record.
/// </summary>
public sealed class OrderRecordEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the order record.
	/// </summary>
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public required Guid Id { get; init; }

	/// <summary>
	/// Gets or initializes the latitude of the order delivery position.
	/// </summary>
	[BsonElement("latitude")]
	public required double Latitude { get; init; }

	/// <summary>
	/// Gets or initializes the longitude of the order delivery position.
	/// </summary>
	[BsonElement("longitude")]
	public required double Longitude { get; init; }

	/// <summary>
	/// Gets or initializes the order subtotal amount before taxes.
	/// </summary>
	[BsonElement("subtotal")]
	public required decimal Subtotal { get; init; }

	/// <summary>
	/// Gets or initializes the composite tax rate applied to the order.
	/// </summary>
	[BsonElement("compositeTaxRate")]
	public required decimal CompositeTaxRate { get; init; }

	/// <summary>
	/// Gets or initializes the calculated total tax amount for the order.
	/// </summary>
	[BsonElement("taxAmount")]
	public required decimal TaxAmount { get; init; }

	/// <summary>
	/// Gets or initializes the total order amount including taxes.
	/// </summary>
	[BsonElement("totalAmount")]
	public required decimal TotalAmount { get; init; }

	/// <summary>
	/// Gets or initializes the detailed breakdown of individual tax rates.
	/// </summary>
	[BsonElement("breakdown")]
	public required OrderTaxBreakdownComponent Breakdown { get; init; }

	/// <summary>
	/// Gets or initializes the list of legal tax jurisdictions applied to the order.
	/// </summary>
	[BsonElement("jurisdictions")]
	public required ICollection<OrderTaxJurisdictionComponent> Jurisdictions { get; init; }

	/// <summary>
	/// Gets or initializes the date and time when the order record was finalized.
	/// </summary>
	[BsonElement("timestamp")]
	[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
	public required DateTime Timestamp { get; init; }
}