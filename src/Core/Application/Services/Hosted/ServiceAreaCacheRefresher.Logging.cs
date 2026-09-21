using System;
using Microsoft.Extensions.Logging;

namespace IWKits.Core.Application.Services.Hosted;

internal sealed partial class ServiceAreaCacheRefresher
{
	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Service area cache refresher background service is starting.")]
	private static partial void LogServiceStarting(ILogger logger);

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Starting periodic refresh of service area cache..")]
	private static partial void LogRefreshStarting(ILogger logger);

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Service area cache successfully refreshed.")]
	private static partial void LogRefreshCompleted(ILogger logger);

	[LoggerMessage(
		Level = LogLevel.Error,
		Message = "An error occurred while refreshing service area cache.")]
	private static partial void LogRefreshFailed(ILogger logger, Exception ex);

	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Service area cache refresher background service is stopping.")]
	private static partial void LogServiceStopping(ILogger logger);
}