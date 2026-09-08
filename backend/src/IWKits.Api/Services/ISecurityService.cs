using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public interface ISecurityService
{
	string GenerateAccessToken(UserInfo userInfo);

	string GenerateRefreshToken();

	string HashPassword(string password);

	bool VerifyPassword(string hashpass, string password);
}