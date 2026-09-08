using System.ComponentModel.DataAnnotations;

namespace IWKits.Api.Settings;

public sealed class ServiceSettings
{
	public const string SectionName = "Services";

	[Required, RegularExpression("default|fake")]
	public string TaxApplier { get; set; } = string.Empty;

	[Required, RegularExpression("default|fake")]
	public string OrderProcess { get; set; } = string.Empty;

	[Required, RegularExpression("default|fake")]
	public string GeoLocation { get; set; } = string.Empty;
}