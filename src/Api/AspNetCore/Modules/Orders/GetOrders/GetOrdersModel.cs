using System;
using IWKits.Core.Common.Models;

namespace IWKits.Api.AspNetCore.Modules.Orders.GetOrders;

internal sealed class GetOrdersModel
{
	public decimal? MinTotalAmount { get; init; }
	public decimal? MaxTotalAmount { get; init; }
	public DateTime? After { get; init; }
	public DateTime? Before { get; init; }
	public OrderRecordSortFields? SortBy { get; init; }
	public SortDirection? SortDirection { get; init; }
	public int? Page { get; init; }
	public int? Size { get; init; }
}