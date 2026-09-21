using System.Collections.Generic;
using IWKits.Core.Common.Messages;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Modules.Orders.Models;

/// <summary>
/// API response containing a paginated collection of order records.
/// </summary>
/// <param name="Data">The collection of order domain records for the current page.</param>
/// <param name="Page">The current retrieved page index.</param>
/// <param name="Size">The maximum number of records requested per page.</param>
/// <param name="ItemsCount">The total number of records matching the filter criteria in the database.</param>
public sealed record ListOrdersResponse(
	IEnumerable<OrderRecord> Data,
	int Page,
	int Size,
	int ItemsCount
) : Response<IEnumerable<OrderRecord>>(Data);