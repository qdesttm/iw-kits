using IWKits.Core.Application.Modules.Orders.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace IWKits.Api.AspNetCore.Modules.Orders.AddOrder;

[Mapper]
internal static partial class AddOrderModelAdapter
{
	public static partial AddOrderRequest ToAddOrderRequest(this AddOrderModel model);
}