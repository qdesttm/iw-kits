using System;

namespace IWKits.Core.Application.Options;

public sealed class SecurityTokensOptions
{
	public const string SectionName = "securityTokens";

	public string JwtIssuer { get; init; }

	public string JwtAudience { get; init; }

	public string JwtKey { get; init; }

	public TimeSpan AccessTokenLifetime { get; init; }

	public TimeSpan RefreshTokenLifetime { get; init; }
}