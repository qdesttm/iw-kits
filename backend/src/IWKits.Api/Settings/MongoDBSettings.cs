using System.ComponentModel.DataAnnotations;

namespace IWKits.Api.Settings;

public sealed class MongoDBSettings
{
	public const string SectionName = "MongoDB";

	[Required]
	public string AuthSource { get; set; } = "admin";

	[Required, Range(8, 512)]
	public int MaxPoolSize { get; set; } = 0;

	[Required]
	public DatabasesNameSettings Databases { get; set; } = new();
}