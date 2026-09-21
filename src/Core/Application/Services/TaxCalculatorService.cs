using System;
using System.Linq;
using IWKits.Core.Application.Extensions;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Models;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal sealed class TaxCalculatorService : ITaxCalculatorService
{
	public TaxCalculationContext Calculate(TaxRate taxRate, Location location, decimal subtotal)
	{
		if (taxRate.ZipCode != location.ZipCode)
			throw new PostalCodeMismatchException(taxRate.ZipCode, location.ZipCode);

		var compositeRate = taxRate.EstimatedCombinedRate;

		var taxAmount = Math.Round(subtotal * compositeRate, 2, MidpointRounding.AwayFromZero);
		var totalAmount = subtotal + taxAmount;

		var jurisdictions = Enum.GetValues<JurisdictionType>()
			.Where(j => taxRate.GetRate(j) > 0)
			.Select(j => new OrderTaxJurisdiction(location.GetName(j), j, taxRate.GetRate(j)))
			.ToList();

		return new TaxCalculationContext(
			compositeRate,
			taxAmount,
			totalAmount,
			taxRate.ToOrderTaxBreakdown(),
			jurisdictions);
	}
}