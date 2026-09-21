using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Common;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Messages;
using IWKits.Core.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IWKits.Api.AspNetCore.Infrastructure.Errors;

internal sealed partial class ServiceExceptionHandler(
	ILogger<ServiceExceptionHandler> logger) :
	IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		ErrorInfo? errorInfo;
		int statusCode;

		switch (exception)
		{
			case ServiceException serviceException:
				statusCode = MapServiceExceptionStatusCode(serviceException.Info.Code);
				errorInfo = ErrorInfoFactory.CreateInternalServerError();

				break;

			case ValidationException validationException:
				statusCode = StatusCodes.Status400BadRequest;
				errorInfo = ErrorInfoFactory.CreateBadRequest(validationException.Message);

				break;

			case BadHttpRequestException badHttpException:
				statusCode = StatusCodes.Status400BadRequest;
				errorInfo = ErrorInfoFactory.CreateBadRequest(badHttpException.Message);

				break;

			default:
				statusCode = StatusCodes.Status500InternalServerError;
				errorInfo = ErrorInfoFactory.CreateInternalServerError();

				LogUnhandledException(exception);

				break;
		}

		httpContext.Response.StatusCode = statusCode;
		var errorResponse = new ErrorResponse(errorInfo);

		await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

		return true;
	}

	private static int MapServiceExceptionStatusCode(string errorCode)
	{
		return errorCode switch
		{
			ErrorCode.InvalidCredentials or
			ErrorCode.SessionNotFound or
			ErrorCode.SessionExpired => StatusCodes.Status401Unauthorized,

			ErrorCode.UserNotFound => StatusCodes.Status404NotFound,

			_ => StatusCodes.Status400BadRequest
		};
	}
}