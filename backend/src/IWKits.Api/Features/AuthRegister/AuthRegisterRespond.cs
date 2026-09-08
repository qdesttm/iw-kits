using System.Text.Json.Serialization;
using IWKits.Api.Entities;

namespace IWKits.Api.Features.AuthRegister;

public sealed record AuthRegisterRespond
(
	[property: JsonPropertyName("access_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? AccessToken = null,

	[property: JsonPropertyName("refresh_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? RefreshToken = null,

	[property: JsonPropertyName("user")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	UserInfo? User = null,

	[property: JsonPropertyName("error_message")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? ErrorMessage = null
);