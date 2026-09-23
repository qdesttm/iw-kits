using System;
using Microsoft.Extensions.Logging;

namespace IWKits.Api.AspNetCore.Infrastructure.Errors;

internal sealed partial class ServiceExceptionHandler
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Error,
		Message = "An unhandled exception occurred during request execution.")]
	private partial void LogUnhandledException(Exception exception);
}