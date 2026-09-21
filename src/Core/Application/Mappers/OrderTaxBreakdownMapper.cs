using IWKits.Core.Common.Models;
using IWKits.Core.Data.Components;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class OrderTaxBreakdownMapper
{
	public static partial OrderTaxBreakdownComponent ToOrderTaxBreakdownComponent(this OrderTaxBreakdown breakdown);
}