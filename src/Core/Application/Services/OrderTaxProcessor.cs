using System;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Models;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Models;
using NetTopologySuite.Geometries;

namespace IWKits.Core.Application.Services;

internal sealed class OrderTaxProcessor(
	IServiceAreaLocator areaLocator,
	IGeoLocationResolver locationResolver,
	ITaxRateProvider taxRateProvider,
	ITaxCalculatorService taxCalculator) :
	IOrderTaxProcessor
{
	public async Task<OrderRecord> ProcessAsync(RawOrderRecord rawOrder, CancellationToken cancellationToken)
	{
		var coordinates = new Point(rawOrder.Longitude, rawOrder.Latitude);

		var serviceArea = await areaLocator.FindAreaAsync(coordinates)
			?? throw new OutsideServiceAreaException();

		var location = await locationResolver.FindLocationAsync(coordinates, serviceArea.StateId, cancellationToken)
			?? throw new JurisdictionNotResolvedException(serviceArea.StateId);

		var taxRate = await taxRateProvider.FindTaxRateAsync(location.ZipCode, serviceArea.StateId, cancellationToken)
			?? throw new TaxDataUnavailableException(location.ZipCode);

		var taxContext = taxCalculator.Calculate(taxRate, location, rawOrder.Subtotal);

		return new OrderRecord(
			Guid.NewGuid(),
			rawOrder.Latitude,
			rawOrder.Longitude,
			rawOrder.Subtotal,
			taxContext.CompositeTaxRate,
			taxContext.TaxAmount,
			taxContext.TotalAmount,
			taxContext.Breakdown,
			taxContext.Jurisdictions,
			rawOrder.Timestamp
		);
	}
}