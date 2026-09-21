using System;

namespace IWKits.Core.Application.Options;

/// <summary>
/// Represents configuration options for application caching layers.
/// </summary>
public sealed class CacheOptions
{
	/// <summary>
	/// The name of the configuration section.
	/// </summary>
	public const string SectionName = "cache";

	/// <summary>
	/// Gets or initializes the absolute expiration lifetime for cached tax rates.
	/// </summary>
	public TimeSpan TaxRateCacheExpiration { get; init; }
}