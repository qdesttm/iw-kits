namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents tax rates associated with a specific postal code.
/// </summary>
/// <param name="ZipCode">The numerical postal or ZIP code.</param>
/// <param name="StateId">The state identifier.</param>
/// <param name="StateRate">The sales tax rate for the state.</param>
/// <param name="EstimatedCombinedRate">The total combined tax rate (state + county + city + special).</param>
/// <param name="EstimatedCountyRate">The tax rate for the county.</param>
/// <param name="EstimatedCityRate">The tax rate for the city.</param>
/// <param name="EstimatedSpecialRate">The tax rate for special local districts.</param>
public sealed record TaxRate(
	int ZipCode,
	string StateId,
	decimal StateRate,
	decimal EstimatedCombinedRate,
	decimal EstimatedCountyRate,
	decimal EstimatedCityRate,
	decimal EstimatedSpecialRate);