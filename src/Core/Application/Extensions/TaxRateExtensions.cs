using System;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="TaxRate"/> domain model.
/// </summary>
public static class TaxRateExtensions
{
	/// <summary>
	/// Resolves the specific tax rate percentage from the specified <see cref="TaxRate"/> based on the <see cref="JurisdictionType"/>.
	/// </summary>
	/// <param name="taxRate">The tax rate reference data instance.</param>
	/// <param name="jurisdictionType">The administrative type of the jurisdiction.</param>
	/// <returns>The numerical tax rate value corresponding to the specified jurisdiction.</returns>
	/// <exception cref="NotSupportedException">Thrown when the provided <see cref="JurisdictionType"/> is invalid.</exception>
	public static decimal GetRate(this TaxRate taxRate, JurisdictionType jurisdictionType)
	{
		return jurisdictionType switch
		{
			JurisdictionType.State => taxRate.StateRate,
			JurisdictionType.County => taxRate.EstimatedCountyRate,
			JurisdictionType.City => taxRate.EstimatedCityRate,
			JurisdictionType.Special => taxRate.EstimatedSpecialRate,
			_ => throw new NotSupportedException($"The jurisdiction type '{jurisdictionType}' is not "
												+ $"supported by the {nameof(TaxRate)} layer.")
		};
	}
}