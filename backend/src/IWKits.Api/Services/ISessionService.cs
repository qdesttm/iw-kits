using System.Threading.Tasks;
using IWKits.Api.Entities;
using System.Threading;

namespace IWKits.Api.Services;

public interface ISessionService
{
	Task<CreateSessionResult> CreateSessionAsync(UserInfo user, CancellationToken ct);

	Task<RefreshSessionResult> RefreshSessionAsync(string refreshToken, CancellationToken ct);
}