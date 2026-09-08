using System.Text.Json.Serialization;

namespace IWKits.Api.Features.CreateOrder;

public sealed record CreateOrderRequest
(
	[property: JsonPropertyName("longitude")]
	double Longitude,

	[property: JsonPropertyName("latitude")]
	double Latitude,

	[property: JsonPropertyName("subtotal")]
	decimal Subtotal
);