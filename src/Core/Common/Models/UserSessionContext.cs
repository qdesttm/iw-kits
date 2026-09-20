namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents the result of a successful user session initialization or refresh, containing token details.
/// </summary>
/// <param name="Session">The active user session domain model.</param>
/// <param name="AccessToken">The newly generated JWT access token string.</param>
public sealed record UserSessionContext(UserSession Session, string AccessToken);