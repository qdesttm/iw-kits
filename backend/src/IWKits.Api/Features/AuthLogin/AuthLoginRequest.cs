using System.Text.Json.Serialization;

namespace IWKits.Api.Features.AuthLogin;

public sealed record AuthLoginRequest
(
	[property: JsonPropertyName("username")]
	string Username,

	[property: JsonPropertyName("password")]
	string Password
);