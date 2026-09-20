using NetTopologySuite.Geometries;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a service area boundary within the application domain.
/// </summary>
/// <param name="StateId">The identifier of the state or region.</param>
/// <param name="Boundary">The multi-polygon representing the geographical boundaries of the service area.</param>
public sealed record ServiceArea(string StateId, MultiPolygon Boundary);