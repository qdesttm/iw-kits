using System;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IWKits.Core.Application.Services.Hosted;

internal sealed partial class ServiceAreaCacheRefresher(
	IServiceAreaLocator areaLocator,
	IOptions<BackgroundServicesOptions> options,
	ILogger<ServiceAreaCacheRefresher> logger) :
	BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		LogServiceStarting(logger);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				LogRefreshStarting(logger);

				await areaLocator.RefreshAreasAsync(stoppingToken);

				LogRefreshCompleted(logger);

				await Task.Delay(options.Value.ServiceAreaRefreshInterval, stoppingToken);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex)
			{
				LogRefreshFailed(logger, ex);
			}
		}

		LogServiceStopping(logger);
	}
}