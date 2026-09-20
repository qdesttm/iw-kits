using System;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Options;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IWKits.Core.Application.Services;

internal sealed class UserSessionService(
	MongoDbContext dbContext,
	ISecurityTokensService tokensService,
	IOptions<SecurityTokensOptions> options) :
	IUserSessionService
{
	public async Task<UserSessionContext> CreateSessionAsync(User user, CancellationToken cancellationToken)
	{
		var accessToken = tokensService.GenerateAccessToken(user);
		var refreshToken = tokensService.GenerateRefreshToken();

		var expiresAt = DateTime.UtcNow + options.Value.RefreshTokenLifetime;

		var sessionEntity = new UserSessionEntity
		{
			Id = Guid.NewGuid(),
			UserId = user.Id,
			RefreshToken = refreshToken,
			ExpiresAt = expiresAt
		};

		await dbContext.UserSessions.InsertOneAsync(sessionEntity, null, cancellationToken);

		var userSession = sessionEntity.ToUserSession();
		return new UserSessionContext(userSession, accessToken);
	}

	public async Task<UserSessionContext> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken)
	{
		var sessionFilter = Builders<UserSessionEntity>.Filter.Eq(s => s.RefreshToken, refreshToken);

		var oldSession = await dbContext.UserSessions
				.Find(sessionFilter)
				.FirstOrDefaultAsync(cancellationToken)
			?? throw new SessionNotFoundException();

		if (oldSession.ExpiresAt < DateTime.UtcNow)
		{
			await dbContext.UserSessions.DeleteOneAsync(sessionFilter, cancellationToken);
			throw new SessionExpiredException();
		}

		var userFilter = Builders<UserEntity>.Filter.Eq(x => x.Id, oldSession.UserId);

		var userEntity = await dbContext.Users
				.Find(userFilter)
				.FirstOrDefaultAsync(cancellationToken)
			?? throw new UserNotFoundException(oldSession.UserId);

		var user = userEntity.ToUser();

		await dbContext.UserSessions.DeleteOneAsync(sessionFilter, cancellationToken);

		return await CreateSessionAsync(user, cancellationToken);
	}
}