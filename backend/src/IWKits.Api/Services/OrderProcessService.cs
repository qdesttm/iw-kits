using NetTopologySuite.Geometries;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using System;

namespace IWKits.Api.Services;

public sealed class OrderProcessService : IOrderProcessService
{
	private readonly IGeoLocationService geoLocation;
	private readonly ITaxApplierService taxApplier;

	public OrderProcessService(IGeoLocationService geoLocation, ITaxApplierService taxApplier)
	{
		this.geoLocation = geoLocation;
		this.taxApplier = taxApplier;
	}

	public async Task<OrderProcessResult> ProcessAsync(RawOrderInfo rawOrder)
	{
		var coordinates = new Point(rawOrder.Longitude, rawOrder.Latitude);
		var ordId = rawOrder.Id;

		var serviceArea = await geoLocation.FindServiceAreaAsync(coordinates);

		if ( serviceArea is null || serviceArea.StateId != "NY" )
		{
			return OrderProcessResult.Failure(
				$"Id({ordId}): Selected location is outside service area.");
		}

		var geoZoneInfo = await geoLocation.FindGeoZoneInfoAsync(coordinates, serviceArea.StateId);

		if ( geoZoneInfo is null )
		{
			return OrderProcessResult.Failure(
				$"Id({ordId}): Unable to calculate tax information for the" +
				$" selected location in '{serviceArea.StateId}'.");
		}

		var taxRate = await geoLocation.FindTaxRateInfoAsync(geoZoneInfo.ZipCode, serviceArea.StateId);

		if ( taxRate is null )
		{
			return OrderProcessResult.Failure(
				$"Id({ordId}): Tax data is unavailable for the identified area ({geoZoneInfo.ZipCode}).");
		}

		var appliedTax = taxApplier.Apply(taxRate, geoZoneInfo, rawOrder.Subtotal);

		if ( appliedTax.HasError )
		{
			return OrderProcessResult.Failure(appliedTax.ErrorMessage);
		}

		return OrderProcessResult.Success(new()
		{
			Id = Guid.NewGuid(),

			Latitude  = rawOrder.Latitude,
			Longitude = rawOrder.Longitude,
			Subtotal  = rawOrder.Subtotal,

			CompositeTaxRate = appliedTax.CompositeTaxRate,
			TaxAmount        = appliedTax.TaxAmount,
			TotalAmount      = appliedTax.TotalAmount,
			Breakdown        = appliedTax.Breakdown,
			Jurisdictions    = appliedTax.Jurisdictions,

			Timestamp = rawOrder.Timestamp
		});
	}
}