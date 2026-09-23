using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class TaxRateMapper
{
	[MapperIgnoreSource(nameof(TaxRateEntity.Id))]
	public static partial TaxRate ToTaxRate(this TaxRateEntity entity);

	[MapProperty(nameof(TaxRate.EstimatedSpecialRate), nameof(OrderTaxBreakdown.SpecialRate))]
	[MapProperty(nameof(TaxRate.EstimatedCountyRate), nameof(OrderTaxBreakdown.CountyRate))]
	[MapProperty(nameof(TaxRate.EstimatedCityRate), nameof(OrderTaxBreakdown.CityRate))]
	[MapperIgnoreSource(nameof(TaxRate.EstimatedCombinedRate))]
	[MapperIgnoreSource(nameof(TaxRate.ZipCode))]
	[MapperIgnoreSource(nameof(TaxRate.StateId))]
	public static partial OrderTaxBreakdown ToOrderTaxBreakdown(this TaxRate taxRate);
}