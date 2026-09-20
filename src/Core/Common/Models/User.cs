using System;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a user within the application domain.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Username">The unique username used for authentication.</param>
/// <param name="PasswordHash">The secure cryptographic hash of the user's password.</param>
/// <param name="Role">The system or domain role assigned to the user.</param>
public sealed record User(Guid Id, string Username, string PasswordHash, string Role);