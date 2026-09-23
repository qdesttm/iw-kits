using System.Collections.Generic;
using IWKits.Core.Common.Messages;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Modules.Orders.Models;

/// <summary>
/// API response containing the summary details of a bulk import operation.
/// </summary>
/// <param name="ImportedTotal">The total number of successfully processed and inserted orders.</param>
/// <param name="Errors">The list of error messages for orders that failed tax calculation.</param>
public sealed record ImportOrdersResponse(int ImportedTotal, IEnumerable<ErrorInfo> Errors) : Response;
