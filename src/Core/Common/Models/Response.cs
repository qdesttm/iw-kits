namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a unified API response envelope encapsulating data or error information.
/// </summary>
/// <typeparam name="T">The type of the payload data contained in the response.</typeparam>
/// <param name="Data">The payload data returned upon a successful operation.</param>
/// <param name="Error">The structured error details if the operation failed.</param>
public abstract record Response<T>(T? Data, ErrorInfo? Error);