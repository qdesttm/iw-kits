using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal interface ITaxCalculatorService
{
	TaxCalculationContext Calculate(TaxRate taxRate, Location location, decimal subtotal);
}
