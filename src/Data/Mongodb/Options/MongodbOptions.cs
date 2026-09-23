namespace IWKits.Core.Mongodb.Options;

/// <summary>
/// Represents configuration options for the MongoDB database connection.
/// </summary>
public sealed class MongodbOptions
{
	/// <summary>
	/// The name of the configuration section.
	/// </summary>
	public const string SectionName = "mongodbOptions";

	/// <summary>
	/// Gets or initializes the full MongoDB server connection string.
	/// </summary>
	public required string ConnectionString { get; init; }

	/// <summary>
	/// Gets or initializes the target MongoDB database name.
	/// </summary>
	public required string DatabaseName { get; init; }
}