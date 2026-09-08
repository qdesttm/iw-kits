using System.Threading.Tasks;
using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public interface IOrderProcessService
{
	Task<OrderProcessResult> ProcessAsync(RawOrderInfo rawOrder);
}