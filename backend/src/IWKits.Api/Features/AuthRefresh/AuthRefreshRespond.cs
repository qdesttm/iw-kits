using System.Text.Json.Serialization;

namespace IWKits.Api.Features.AuthRefresh;

public sealed record AuthRefreshRespond
(
	[property: JsonPropertyName("access_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? AccessToken = null,

	[property: JsonPropertyName("refresh_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? RefreshToken = null,

	[property: JsonPropertyName("error_message")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? ErrorMessage = null
);