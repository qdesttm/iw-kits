using System.Text.Json.Serialization;

namespace IWKits.Api.Features.AuthRefresh;

public sealed record AuthRefreshRequest
(
	[property: JsonPropertyName("refresh_token")]
	string RefreshToken
);