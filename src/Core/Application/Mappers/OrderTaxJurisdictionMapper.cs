using IWKits.Core.Common.Models;
using IWKits.Core.Data.Components;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class OrderTaxJurisdictionMapper
{
	public static partial OrderTaxJurisdictionComponent ToOrderTaxJurisdictionComponent(this OrderTaxJurisdiction jurisdiction);
}