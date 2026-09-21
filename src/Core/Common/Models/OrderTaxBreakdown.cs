namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents the individual tax rate breakdown of an order.
/// </summary>
/// <param name="StateRate">The state-level tax rate component.</param>
/// <param name="CountyRate">The county-level tax rate component.</param>
/// <param name="CityRate">The city-level tax rate component.</param>
/// <param name="SpecialRate">The special local district tax rate component.</param>
public sealed record OrderTaxBreakdown(
	decimal StateRate,
	decimal CountyRate,
	decimal CityRate,
	decimal SpecialRate);