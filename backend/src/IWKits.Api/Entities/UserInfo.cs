using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using System;

namespace IWKits.Api.Entities;

public sealed record UserInfo
{
	[BsonElement("_id")]
	[JsonPropertyName("id")]
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; init; } = Guid.Empty;

	[BsonElement("username")]
	[JsonPropertyName("username")]
	public string Username { get; init; } = string.Empty;

	[BsonElement("password")]
	[JsonPropertyName("password")]
	public string Password { get; init; } = string.Empty;

	[BsonElement("role")]
	[JsonPropertyName("role")]
	public string Role { get; init; } = string.Empty;
}