namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents information about an application or API error.
/// </summary>
/// <param name="Code">The unique machine-readable error code.</param>
/// <param name="Message">The human-readable description of the error.</param>
public sealed record ErrorInfo(string Code, string Message);