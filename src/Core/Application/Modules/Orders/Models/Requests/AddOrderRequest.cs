using MediatR;

namespace IWKits.Core.Application.Modules.Orders.Models.Requests;

/// <summary>
/// Request to create a new order with tax evaluation.
/// </summary>
/// <param name="Longitude">The longitude coordinate of the delivery address.</param>
/// <param name="Latitude">The latitude coordinate of the delivery address.</param>
/// <param name="Subtotal">The order subtotal amount before taxes.</param>
public sealed record AddOrderRequest(
	double Longitude,
	double Latitude,
	decimal Subtotal
) : IRequest<AddOrderResponse>;
