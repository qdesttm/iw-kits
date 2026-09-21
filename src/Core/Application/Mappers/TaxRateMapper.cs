using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class TaxRateMapper
{
	[MapperIgnoreSource(nameof(TaxRateEntity.Id))]
	public static partial TaxRate ToTaxRate(this TaxRateEntity entity);
}