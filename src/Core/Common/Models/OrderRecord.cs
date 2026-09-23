using System;
using System.Collections.Generic;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a finalized order financial and tax record.
/// </summary>
/// <param name="Id">The unique identifier for the order record.</param>
/// <param name="Latitude">The latitude of the order delivery position.</param>
/// <param name="Longitude">The longitude of the order delivery position.</param>
/// <param name="Subtotal">The order subtotal amount before taxes.</param>
/// <param name="CompositeTaxRate">The composite tax rate applied to the order.</param>
/// <param name="TaxAmount">The calculated total tax amount for the order.</param>
/// <param name="TotalAmount">The total order amount including taxes.</param>
/// <param name="Breakdown">The detailed breakdown of individual tax rates.</param>
/// <param name="Jurisdictions">The list of legal tax jurisdictions applied to the order.</param>
/// <param name="Timestamp">The date and time when the order record was finalized.</param>
public sealed record OrderRecord(
	Guid Id,
	double Latitude,
	double Longitude,
	decimal Subtotal,
	decimal CompositeTaxRate,
	decimal TaxAmount,
	decimal TotalAmount,
	OrderTaxBreakdown Breakdown,
	IReadOnlyList<OrderTaxJurisdiction> Jurisdictions,
	DateTime Timestamp);