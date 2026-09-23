using System;
using IWKits.Core.Common.Models;
using MediatR;

namespace IWKits.Core.Application.Modules.Orders.Models.Requests;

/// <summary>
/// Request to retrieve a paginated and filtered list of order records.
/// </summary>
/// <param name="MinTotalAmount">The minimum threshold for the total order amount.</param>
/// <param name="MaxTotalAmount">The maximum threshold for the total order amount.</param>
/// <param name="After">Filter records created after this specific date and time.</param>
/// <param name="Before">Filter records created before this specific date and time.</param>
/// <param name="SortBy">The field used to sort the resulting collection.</param>
/// <param name="SortDirection">The direction applied to the sorting logic.</param>
/// <param name="Page">The target page index to fetch.</param>
/// <param name="Size">The maximum number of records to return on a single page.</param>
public sealed record ListOrdersRequest(
	decimal? MinTotalAmount = null,
	decimal? MaxTotalAmount = null,
	DateTime? After = null,
	DateTime? Before = null,
	OrderRecordSortFields? SortBy = null,
	SortDirection? SortDirection = null,
	int? Page = null,
	int? Size = null
) : IRequest<ListOrdersResponse>;