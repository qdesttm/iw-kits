using System.Diagnostics.CodeAnalysis;
using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public sealed record RefreshSessionResult
{
	[MemberNotNullWhen(false, nameof(Session))]
	[MemberNotNullWhen(false, nameof(AccessToken))]
	[MemberNotNullWhen(true, nameof(ErrorMessage))]
	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

	public string? ErrorMessage { get; init; }

	public SessionInfo? Session { get; init; }
	public string? AccessToken { get; init; }

	public static RefreshSessionResult Success(SessionInfo session, string token)
		=> new() { Session = session, AccessToken = token };

	public static RefreshSessionResult Failure(string errorMsg) => new() { ErrorMessage = errorMsg };
}