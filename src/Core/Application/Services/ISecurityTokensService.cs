using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Services;

public interface ISecurityTokensService
{
	string GenerateAccessToken(User user);

	string GenerateRefreshToken();
}