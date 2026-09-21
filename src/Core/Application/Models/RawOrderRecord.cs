using System;
using System.Text.Json.Serialization;

namespace IWKits.Core.Application.Models;

/// <summary>
/// Represents the raw, unprocessed order data received from external inputs like API requests or CSV imports.
/// </summary>
public sealed class RawOrderRecord
{
	/// <summary>
	/// Gets or initializes the latitude coordinate of the delivery address.
	/// </summary>
	[JsonPropertyName("latitude")]
	[CsvHelper.Configuration.Attributes.Name("latitude")]
	public required double Latitude { get; init; }

	/// <summary>
	/// Gets or initializes the longitude coordinate of the delivery address.
	/// </summary>
	[JsonPropertyName("longitude")]
	[CsvHelper.Configuration.Attributes.Name("longitude")]
	public required double Longitude { get; init; }

	/// <summary>
	/// Gets or initializes the order subtotal financial amount before taxes.
	/// </summary>
	[JsonPropertyName("subtotal")]
	[CsvHelper.Configuration.Attributes.Name("subtotal")]
	public required decimal Subtotal { get; init; }

	/// <summary>
	/// Gets or initializes the timestamp indicating when the order was originally submitted.
	/// </summary>
	[JsonPropertyName("timestamp")]
	[CsvHelper.Configuration.Attributes.Name("timestamp")]
	public required DateTime Timestamp { get; init; }
}