using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public interface ITaxApplierService
{
	TaxApplyResult Apply(TaxRateInfo taxRate, GeoZoneInfo geoZone, decimal subtotal);
}