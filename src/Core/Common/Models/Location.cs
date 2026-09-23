using NetTopologySuite.Geometries;

namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a geographical and postal location.
/// </summary>
/// <param name="StateId">The unique state identifier.</param>
/// <param name="ZipCode">The numerical postal or ZIP code.</param>
/// <param name="StateName">The full name of the state.</param>
/// <param name="CityName">The name of the city.</param>
/// <param name="CountyName">The name of the county.</param>
/// <param name="Coordinates">The coordinates representing the exact GPS point (X for Longitude, Y for Latitude).</param>
public sealed record Location(
	string StateId,
	int ZipCode,
	string StateName,
	string CityName,
	string CountyName,
	Coordinate Coordinates);