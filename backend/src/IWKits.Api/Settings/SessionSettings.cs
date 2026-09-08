using System.ComponentModel.DataAnnotations;

namespace IWKits.Api.Settings;

public sealed class SessionSettings
{
	public const string SectionName = "Session";

	[Required, Range(1, int.MaxValue)]
	public int AccessPeriod { get; set; } = int.MinValue;

	[Required, Range(1, int.MaxValue)]
	public int RefreshPeriod { get; set; } = int.MaxValue;
}