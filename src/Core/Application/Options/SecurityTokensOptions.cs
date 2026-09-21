using System;

namespace IWKits.Core.Application.Options;

public sealed class SecurityTokensOptions
{
	/// <summary>
	/// The name of the configuration section.
	/// </summary>
	public const string SectionName = "securityTokens";

	public string JwtIssuer { get; init; } = string.Empty;

	public string JwtAudience { get; init; } = string.Empty;

	public string JwtKey { get; init; } = string.Empty;

	public TimeSpan AccessTokenLifetime { get; init; }

	public TimeSpan RefreshTokenLifetime { get; init; }
}