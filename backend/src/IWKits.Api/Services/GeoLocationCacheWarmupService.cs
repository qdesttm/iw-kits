using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace IWKits.Api.Services;

public sealed class GeoLocationCacheWarmupService : BackgroundService
{
	private static readonly TimeSpan RefreshTime = TimeSpan.FromHours(1);

	private readonly ILogger<GeoLocationCacheWarmupService> logger;
	private readonly IGeoLocationService geoLocation;

	public GeoLocationCacheWarmupService(IGeoLocationService geoLocation,
		ILogger<GeoLocationCacheWarmupService> logger) : base()
	{
		this.geoLocation = geoLocation;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while ( !stoppingToken.IsCancellationRequested )
		{
			logger.LogInformation("Refreshing geo location service cache.");

			await geoLocation.RefreshGeoLocationCache();
			await Task.Delay(RefreshTime, stoppingToken);
		}
	}
}