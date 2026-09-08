using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public sealed record CreateSessionResult
(
	SessionInfo Session,
	string AccessToken
);