using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

internal interface IUserSessionService
{
	Task<UserSessionContext> CreateSessionAsync(User user, CancellationToken cancellationToken);

	Task<UserSessionContext> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken);
}
