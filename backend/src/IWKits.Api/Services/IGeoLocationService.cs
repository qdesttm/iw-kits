using NetTopologySuite.Geometries;
using System.Threading.Tasks;
using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public interface IGeoLocationService
{
	Task<ServiceArea?> FindServiceAreaAsync(Point coordinates);

	Task<GeoZoneInfo?> FindGeoZoneInfoAsync(Point coordinates, string state);

	Task<TaxRateInfo?> FindTaxRateInfoAsync(int zipCode, string state);

	Task RefreshGeoLocationCache();
}