using System;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a user profile.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Username">The unique username of the profile.</param>
/// <param name="Role">The system or domain role assigned to the user.</param>
public sealed record UserProfile(Guid Id, string Username, string Role);