using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for a user session.
/// </summary>
public sealed class SessionEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the session.
	/// </summary>
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public required Guid Id { get; init; }

	/// <summary>
	/// Gets or initializes the unique identifier of the user associated with this session.
	/// </summary>
	[BsonElement("userId")]
	[BsonRepresentation(BsonType.String)]
	public required Guid UserId { get; init; }

	/// <summary>
	/// Gets or initializes the cryptographic refresh token string.
	/// </summary>
	[BsonElement("refreshToken")]
	public required string RefreshToken { get; init; }

	/// <summary>
	/// Gets or initializes the date and time when the session expires.
	/// </summary>
	[BsonElement("expiresAt")]
	[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
	public required DateTime ExpiresAt { get; init; }
}