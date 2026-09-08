using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IWKits.Api.Entities;

[BsonIgnoreExtraElements]
public sealed record TaxRateInfoFull : TaxRateInfo
{
	[BsonElement("state_id")]
	public string StateId { get; init; } = string.Empty;
}