using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class OrderRecordMapper
{
	public static partial OrderRecordEntity ToOrderRecordEntity(this OrderRecord orderRecord);
}