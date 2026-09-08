using NetTopologySuite.Geometries;
using IWKits.Api.Entities;

namespace IWKits.Api.Common;

public sealed record ServiceAreaXNTS
(
	ServiceAreaFull ServiceArea,
	MultiPolygon NtsBoundary
);