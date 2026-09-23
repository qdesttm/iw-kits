namespace IWKits.Core.Common.Models;

/// <summary>
/// Specifies the geographic or administrative level of a tax jurisdiction.
/// </summary>
public enum JurisdictionType
{
	/// <summary>
	/// Represents a state-level tax jurisdiction.
	/// </summary>
	State,

	/// <summary>
	/// Represents a county-level tax jurisdiction.
	/// </summary>
	County,

	/// <summary>
	/// Represents a city-level tax jurisdiction.
	/// </summary>
	City,

	/// <summary>
	/// Represents a special local district or transit authority tax jurisdiction.
	/// </summary>
	Special
}