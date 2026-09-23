using IWKits.Core.Common.Models;

namespace IWKits.Core.Common.Messages;

/// <summary>
/// Represents a unified API response envelope encapsulating operation success or error information.
/// </summary>
/// <param name="Error">The structured error details if the operation failed.</param>
public record Response(ErrorInfo? Error = null);