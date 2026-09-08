using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.GeoJsonObjectModel;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using IWKits.Api.Common;
using MongoDB.Driver;
using System.Linq;
using System;

namespace IWKits.Api.Services;

public sealed class GeoLocationService : IGeoLocationService
{
	const string ServiceAreasCacheKey = "all_service_areas";

	private readonly CoreDatabaseContext coreDatabase;
	private readonly IMemoryCache cache;

	public GeoLocationService(CoreDatabaseContext coreDatabase, IMemoryCache cache)
	{
		this.coreDatabase = coreDatabase;
		this.cache = cache;
	}

	public async Task<ServiceArea?> FindServiceAreaAsync(Point coordinates)
	{
		var wrappedAreas = cache.Get<List<ServiceAreaXNTS>>
			(ServiceAreasCacheKey) ?? [];

		foreach ( var areaXNTS in wrappedAreas )
		{
			if ( areaXNTS.NtsBoundary.Contains(coordinates) )
			{
				return areaXNTS.ServiceArea;
			}
		}

		return null;
	}

	public async Task<GeoZoneInfo?> FindGeoZoneInfoAsync(Point coordinates, string state)
	{
		var gcoordinates = GeoJson.Geographic(coordinates.X, coordinates.Y);

		var filterBuilder = Builders<GeoZoneInfoFull>.Filter;

		var geoZoneFilter = filterBuilder.And
		(
			filterBuilder.NearSphere(x => x.Coordinates, GeoJson.Point(gcoordinates)),
			filterBuilder.        Eq(x => x.StateId, state)
		);

		return await coreDatabase.GeoZones
			.Find(geoZoneFilter)
			.Limit(limit: 1)
			.As<GeoZoneInfo>()
			.FirstOrDefaultAsync();
	}

	public async Task<TaxRateInfo?> FindTaxRateInfoAsync(int zipCode, string state)
	{
		string key = $"{nameof(TaxRateInfo)}_{state}_{zipCode}";

		return await cache.GetOrCreateAsync(key, async (entry) =>
		{
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
			entry.Priority = CacheItemPriority.High;

			var filterBuilder = Builders<TaxRateInfoFull>.Filter;

			var taxRateFilter = filterBuilder.And
			(
				filterBuilder.Eq(x => x.ZipCode, zipCode),
				filterBuilder.Eq(x => x.StateId, state)
			);

			return await coreDatabase.TaxRates
				.Find(taxRateFilter)
				.Limit(limit: 1)
				.As<TaxRateInfo>()
				.FirstOrDefaultAsync();
		});
	}

	public async Task RefreshGeoLocationCache()
	{
		var options = new MemoryCacheEntryOptions()
		{
			Priority = CacheItemPriority.High
		};

		var areas = coreDatabase.SerAreas.Find(_ => true);
		var wrapperAreas = new List<ServiceAreaXNTS>();

		foreach ( var area in await areas.ToListAsync() )
		{
			var multiPolygon = ToMultiPolygon(area.Boundary);
			var areaXNTS = new ServiceAreaXNTS(area, multiPolygon);

			wrapperAreas.Add(areaXNTS);
		}

		cache.Set(ServiceAreasCacheKey, wrapperAreas, options);
	}

	private static MultiPolygon ToMultiPolygon(
		GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> geoJsonMultiPolygon)
	{
		var factory = GeometryFactory.Floating;
		var listPolygons = new List<Polygon>();

		foreach ( var geoJsonPolygon in geoJsonMultiPolygon.Coordinates.Polygons )
		{
			var shellCoords = geoJsonPolygon
				.Exterior
				.Positions
				.Select(ToNtsCoords)
				.ToArray();

			var holes = geoJsonPolygon
				.Holes
				.Select(
					hole => factory.CreateLinearRing
					(
						[.. hole.Positions.Select(ToNtsCoords)]
					)
				)
				.ToArray();

			var shell = factory.CreateLinearRing(shellCoords);
			var polygon = factory.CreatePolygon(shell, holes);

			listPolygons.Add(polygon);
		}

		return factory.CreateMultiPolygon([..listPolygons]);
	}

	private static Coordinate ToNtsCoords(GeoJson2DGeographicCoordinates coordinates)
	{
		return new Coordinate(coordinates.Longitude, coordinates.Latitude);
	}
}