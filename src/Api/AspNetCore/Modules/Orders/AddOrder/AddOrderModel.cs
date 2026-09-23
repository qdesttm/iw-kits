namespace IWKits.Api.AspNetCore.Modules.Orders.AddOrder;

internal sealed class AddOrderModel
{
	public required double Longitude { get; init; }
	public required double Latitude { get; init; }
	public required decimal Subtotal { get; init; }
}