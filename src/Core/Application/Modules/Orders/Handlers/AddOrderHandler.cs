using System;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Models;
using IWKits.Core.Application.Modules.Orders.Models;
using IWKits.Core.Application.Modules.Orders.Models.Requests;
using IWKits.Core.Application.Services;
using IWKits.Core.Data;
using MediatR;

namespace IWKits.Core.Application.Modules.Orders.Handlers;

internal sealed class AddOrderHandler(
	IOrderTaxProcessor taxProcessor,
	MongoDbContext dbContext) :
	IRequestHandler<AddOrderRequest, AddOrderResponse>
{
	public async Task<AddOrderResponse> Handle(AddOrderRequest request, CancellationToken cancellationToken)
	{
		var rawOrder = new RawOrderRecord
		{
			Longitude = request.Longitude,
			Latitude = request.Latitude,
			Subtotal = request.Subtotal,
			Timestamp = DateTime.UtcNow
		};

		var orderRecord = await taxProcessor.ProcessAsync(rawOrder, cancellationToken);
		var recordEntity = orderRecord.ToOrderRecordEntity();

		await dbContext.OrderRecords.InsertOneAsync(recordEntity, null, cancellationToken);

		return new AddOrderResponse(recordEntity.Id);
	}
}