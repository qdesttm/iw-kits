using System;

namespace IWKits.Core.Application.Options;

/// <summary>
/// Represents configuration options for background services and workers.
/// </summary>
public sealed class BackgroundServicesOptions
{
	/// <summary>
	/// The name of the configuration section.
	/// </summary>
	public const string SectionName = "backgroundServices";

	/// <summary>
	/// Gets or initializes the polling interval for the service area cache refresher.
	/// </summary>
	public TimeSpan ServiceAreaRefreshInterval { get; init; }
}