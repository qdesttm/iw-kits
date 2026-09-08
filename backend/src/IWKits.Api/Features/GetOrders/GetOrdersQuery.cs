using Microsoft.AspNetCore.Mvc;
using System;

namespace IWKits.Api.Features.GetOrders;

public sealed record GetOrdersQuery
(
	[property: FromQuery(Name="min_total_amount")]
	decimal? MinTotalAmount,
	[property: FromQuery(Name="max_total_amount")]
	decimal? MaxTotalAmount,

	[property: FromQuery(Name="from_date")]
	DateTime? FromDate,
	[property: FromQuery(Name="to_date")]
	DateTime? ToDate,

	[property: FromQuery(Name="sort_by")]
	string? SortBy,
	[property: FromQuery(Name="descending")]
	bool? Descending,

	[property: FromQuery(Name="page_size")]
	int? PageSize,
	[property: FromQuery(Name="page")]
	int? Page
);