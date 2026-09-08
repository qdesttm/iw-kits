using System.ComponentModel.DataAnnotations;

namespace IWKits.Api.Settings;

public sealed class SecuritySettings
{
	public const string SectionName = "Security";

	[Required, MinLength(5)]
	public string JwtIssuer { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string JwtAudience { get; set; } = string.Empty;
}