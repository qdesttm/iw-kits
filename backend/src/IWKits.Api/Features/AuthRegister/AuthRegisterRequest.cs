using System.Text.Json.Serialization;

namespace IWKits.Api.Features.AuthRegister;

public sealed record AuthRegisterRequest
(
	[property: JsonPropertyName("username")]
	string Username,

	[property: JsonPropertyName("password")]
	string Password
);