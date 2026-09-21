using System;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Location"/>.
/// </summary>
public static class LocationExtensions
{
	/// <summary>
	/// Resolves the jurisdiction name from the specified <see cref="Location"/> based on the <see cref="JurisdictionType"/>.
	/// </summary>
	/// <param name="location">The geographical location instance.</param>
	/// <param name="jurisdictionType">The administrative type of the jurisdiction.</param>
	/// <returns>The localized name of the jurisdiction or a default fallback string.</returns>
	/// <exception cref="NotSupportedException">Thrown when the provided <see cref="JurisdictionType"/> is invalid.</exception>
	public static string GetName(this Location location, JurisdictionType jurisdictionType)
	{
		return jurisdictionType switch
		{
			JurisdictionType.State => location.StateName,
			JurisdictionType.County => location.CountyName,
			JurisdictionType.City => location.CityName,
			JurisdictionType.Special => "Special District",
			_ => throw new NotSupportedException($"The jurisdiction type '{jurisdictionType}' is not "
												+ $"supported by the {nameof(Location)} layer.")
		};
	}
}