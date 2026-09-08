using System.Threading.Tasks;
using IWKits.Api.Settings;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using System.Threading;
using MongoDB.Driver;
using System;

namespace IWKits.Api.Services;

public sealed class SessionService : ISessionService
{
	private readonly AuthDatabaseContext authDatabase;
	private readonly SessionSettings sessionSettings;
	private readonly ISecurityService securityService;

	public SessionService(ISecurityService securityService,
		SessionSettings sessionSettings, AuthDatabaseContext authDatabase)
	{
		this.sessionSettings = sessionSettings;
		this.securityService = securityService;
		this.authDatabase = authDatabase;
	}

	public async Task<CreateSessionResult> CreateSessionAsync(UserInfo userInfo, CancellationToken ct)
	{
		var accessToken = securityService.GenerateAccessToken(userInfo);
		var refreshToken = securityService.GenerateRefreshToken();

		var expiresAt = DateTime.UtcNow.AddMinutes
			(sessionSettings.RefreshPeriod);

		var session = new SessionInfo()
		{
			UserId = userInfo.Id,
			RefreshToken = refreshToken,
			ExpiresAt = expiresAt
		};

		await authDatabase.Sessions.InsertOneAsync(session, null, ct);
		return new CreateSessionResult(session, accessToken);
	}

	public async Task<RefreshSessionResult> RefreshSessionAsync(string refreshToken, CancellationToken ct)
	{
		var sessionFilter = Builders<SessionInfo>.Filter.Eq(s => s.RefreshToken, refreshToken);
		var oldSession = await authDatabase.Sessions.Find(sessionFilter).FirstOrDefaultAsync(ct);

		if ( oldSession is null )
		{
			return RefreshSessionResult.Failure("Session not found");
		}

		if ( oldSession.ExpiresAt < DateTime.UtcNow )
		{
			await authDatabase.Sessions.DeleteOneAsync(sessionFilter, ct);
			return RefreshSessionResult.Failure("Session expired");
		}

		var userFilter = Builders<UserInfo>.Filter.Eq(x => x.Id, oldSession.UserId);
		var user = await authDatabase.Users.Find(userFilter).FirstOrDefaultAsync(ct);

		if ( user is null )
		{
			return RefreshSessionResult.Failure("User associated with session not found");
		}

		await authDatabase.Sessions.DeleteOneAsync(sessionFilter, ct);
		var createResult = await CreateSessionAsync(user, ct);

		return RefreshSessionResult.Success(
			createResult.Session, createResult.AccessToken);
	}
}