namespace IWKits.Core.Application.Services;

/// <summary>
/// Represents a factory to generate consistent cache keys for tracking tax rate models.
/// </summary>
public static class TaxRateCacheKeyBuilder
{
	/// <summary>
	/// The global prefix applied to all tax rate cache records.
	/// </summary>
	public const string Prefix = "tax-rate";

	/// <summary>
	/// Generates a cache key from a zip code and state id.
	/// </summary>
	public static string Create(int zipCode, string state)
		=> $"{Prefix}/{state.ToLowerInvariant()}/{zipCode}";
}