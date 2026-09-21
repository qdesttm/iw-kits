using System;

namespace IWKits.Core.Application.Options;

/// <summary>
/// Represents configuration options for JSON Web Token (JWT) generation and validation.
/// </summary>
public sealed class SecurityTokensOptions
{
	/// <summary>
	/// The name of the configuration section in appsettings.json.
	/// </summary>
	public const string SectionName = "securityTokens";

	/// <summary>
	/// Gets or initializes the valid issuer of the security tokens.
	/// </summary>
	public string JwtIssuer { get; init; } = string.Empty;

	/// <summary>
	/// Gets or initializes the valid audience for the security tokens.
	/// </summary>
	public string JwtAudience { get; init; } = string.Empty;

	/// <summary>
	/// Gets or initializes the secure cryptographic key used to sign and verify tokens.
	/// </summary>
	public string JwtKey { get; init; } = string.Empty;

	/// <summary>
	/// Gets or initializes the lifespan duration of a generated access token.
	/// </summary>
	public TimeSpan AccessTokenLifetime { get; init; }

	/// <summary>
	/// Gets or initializes the lifespan duration of a generated refresh token session.
	/// </summary>
	public TimeSpan RefreshTokenLifetime { get; init; }
}