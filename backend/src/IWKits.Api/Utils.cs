using FluentValidation.Results;
using System.Linq;
using System;

namespace IWKits.Api;

public static class Utils
{
	public static string GetRequiredEnv(string key)
	{
		string? value = Environment.GetEnvironmentVariable(key);
		if ( value is not null ) return value;

		throw new InvalidOperationException
		(
			$"Critical error: '{key}' environment variable is missing. " +
			"Check your .env file or environment settings."
		);
	}

	public static string JoinErrorsOrEmpty(this ValidationResult? result)
	{
		if ( result is null || result.IsValid ) return string.Empty;
		return string.Join('\n', result.Errors.Select(e => e.ErrorMessage));
	}
}