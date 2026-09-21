namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a unified API response envelope encapsulating data or error information.
/// </summary>
/// <typeparam name="T">The type of the payload data contained in the response.</typeparam>
public record Response<T>
{
	/// <summary>
	/// The payload data returned upon a successful operation.
	/// </summary>
	public T? Data { get; init; }

	/// <summary>
	/// The structured error details if the operation failed.
	/// </summary>
	public ErrorInfo? Error { get; init; }
}