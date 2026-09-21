using IWKits.Core.Common.Models;

namespace IWKits.Core.Common.Messages;

/// <summary>
/// Represents a unified API response envelope encapsulating operation success or error information.
/// </summary>
/// <param name="Error">The structured error details if the operation failed.</param>
public record Response(ErrorInfo? Error = null);

/// <summary>
/// Represents a unified API response envelope encapsulating payload data and optional error information.
/// </summary>
/// <typeparam name="T">The type of the payload data contained in the response.</typeparam>
/// <param name="Data">The payload data returned upon a successful operation.</param>
/// <param name="Error">The structured error details if the operation failed.</param>
public record Response<T>(T? Data, ErrorInfo? Error = null) : Response(Error);