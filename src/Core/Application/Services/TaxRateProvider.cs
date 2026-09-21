using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Options;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IWKits.Core.Application.Services;

internal sealed class TaxRateProvider(
	MongoDbContext dbContext,
	IMemoryCache cache,
	IOptions<CacheOptions> options) :
	ITaxRateProvider
{
	public async Task<TaxRate?> FindTaxRateAsync(
		int zipCode,
		string state,
		CancellationToken cancellationToken)
	{
		var cacheKey = TaxRateCacheKeyBuilder.Create(zipCode, state);

		return await cache.GetOrCreateAsync(cacheKey, async entry =>
		{
			entry.AbsoluteExpirationRelativeToNow = options.Value.TaxRateCacheExpiration;

			var filterBuilder = Builders<TaxRateEntity>.Filter;

			var filter = filterBuilder.And(
				filterBuilder.Eq(x => x.ZipCode, zipCode),
				filterBuilder.Eq(x => x.StateId, state)
			);

			var entity = await dbContext.TaxRates
				.Find(filter)
				.FirstOrDefaultAsync(cancellationToken);

			return entity?.ToTaxRate();
		});
	}
}