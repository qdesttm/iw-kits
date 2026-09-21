using System;
using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using NetTopologySuite.Geometries;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class ServiceAreaMapper
{
	[MapPropertyFromSource(nameof(ServiceArea.Boundary), Use = nameof(MapBoundary))]
	[MapperIgnoreSource(nameof(ServiceAreaEntity.Id))]
	public static partial ServiceArea ToServiceArea(this ServiceAreaEntity serviceArea);

	private static MultiPolygon MapBoundary(this ServiceAreaEntity serviceArea)
		=> serviceArea.Boundary.ToMultiPolygon();
}