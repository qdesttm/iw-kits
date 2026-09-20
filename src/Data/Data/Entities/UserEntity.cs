using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IWKits.Core.Data.Entities;

/// <summary>
/// Represents the database entity for a user.
/// </summary>
public sealed class UserEntity
{
	/// <summary>
	/// Gets or initializes the unique identifier for the user.
	/// </summary>
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public required Guid Id { get; init; }

	/// <summary>
	/// Gets or initializes the unique username.
	/// </summary>
	[BsonElement("username")]
	public required string Username { get; init; }

	/// <summary>
	/// Gets or initializes the secure cryptographic hash of the user's password.
	/// </summary>
	[BsonElement("passwordHash")]
	public required string PasswordHash { get; init; }

	/// <summary>
	/// Gets or initializes the user's system or domain role.
	/// </summary>
	[BsonElement("role")]
	public required string Role { get; init; }
}