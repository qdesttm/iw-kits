using IWKits.Core.Common.Models;

namespace IWKits.Core.Common.Messages;

/// <summary>
/// API response envelope containing mandatory error details.
/// </summary>
/// <param name="Error">The structured error details describing the operation failure.</param>
public sealed record ErrorResponse(ErrorInfo Error) : Response(Error);