using System.ComponentModel.DataAnnotations;

namespace IWKits.Api.Settings;

public sealed class DatabasesNameSettings
{
	[Required, MinLength(5)]
	public string Data { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string Auth { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string Core { get; set; } = string.Empty;
}