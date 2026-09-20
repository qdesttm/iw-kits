using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IWKits.Core.Application.Options;
using IWKits.Core.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IWKits.Core.Application.Services;

public sealed class SecurityTokensService(
	IOptions<SecurityTokensOptions> options) :
	ISecurityTokensService
{
	private readonly byte[] _jwtKeyBytes = Encoding.UTF8.GetBytes(options.Value.JwtKey);
	private readonly JsonWebTokenHandler _tokenHandler = new();

	public string GenerateAccessToken(User user)
	{
		var securityKey = new SymmetricSecurityKey(_jwtKeyBytes);
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var expirationDate = DateTime.UtcNow + options.Value.AccessTokenLifetime;

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = options.Value.JwtIssuer,
			Audience = options.Value.JwtAudience,
			Expires = expirationDate,
			SigningCredentials = credentials,
			Subject = new ClaimsIdentity(
			[
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.Username),
				new(ClaimTypes.Role, user.Role)
			])
		};

		return _tokenHandler.CreateToken(tokenDescriptor);
	}

	public string GenerateRefreshToken()
		=> Convert.ToBase64String( RandomNumberGenerator.GetBytes(32) );
}