using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Models;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal interface IOrderTaxProcessor
{
	Task<OrderRecord> ProcessAsync(RawOrderRecord rawOrder, CancellationToken cancellationToken);
}
