using System.Collections.Generic;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Models;

/// <summary>
/// Represents the comprehensive result of a tax calculation process.
/// </summary>
/// <param name="CompositeTaxRate">The total combined tax rate applied.</param>
/// <param name="TaxAmount">The calculated and rounded total tax amount.</param>
/// <param name="TotalAmount">The final total amount including subtotal and tax.</param>
/// <param name="Breakdown">The detailed breakdown of individual tax components.</param>
/// <param name="Jurisdictions">The list of active tax jurisdictions applied to the calculation.</param>
public sealed record TaxCalculationContext(
	decimal CompositeTaxRate,
	decimal TaxAmount,
	decimal TotalAmount,
	OrderTaxBreakdown Breakdown,
	IReadOnlyList<OrderTaxJurisdiction> Jurisdictions);