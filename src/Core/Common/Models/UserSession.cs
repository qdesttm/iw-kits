using System;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents an active user session.
/// </summary>
/// <param name="Id">The unique identifier of the session.</param>
/// <param name="UserId">The unique identifier of the user associated with this session.</param>
/// <param name="RefreshToken">The cryptographic refresh token string.</param>
/// <param name="ExpiresAt">The date and time in UTC when the session expires.</param>
public sealed record UserSession(Guid Id, Guid UserId, string RefreshToken, DateTime ExpiresAt);