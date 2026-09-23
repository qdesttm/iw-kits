using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal interface ITaxRateProvider
{
	Task<TaxRate?> FindTaxRateAsync(
		int zipCode,
		string state,
		CancellationToken cancellationToken);
}
