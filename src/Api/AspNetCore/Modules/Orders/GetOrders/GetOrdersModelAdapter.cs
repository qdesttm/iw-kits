using IWKits.Core.Application.Modules.Orders.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace IWKits.Api.AspNetCore.Modules.Orders.GetOrders;

[Mapper]
internal static partial class GetOrdersModelAdapter
{
	public static partial ListOrdersRequest ToListOrdersRequest(this GetOrdersModel model);
}