namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when an operation fails due to an empty or missing uploaded file.
/// </summary>
public sealed class InvalidFileException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="InvalidFileException"/> instance.
	/// </summary>
	public InvalidFileException()
		: base(ErrorInfoFactory.CreateInvalidFile())
	{
	}
}