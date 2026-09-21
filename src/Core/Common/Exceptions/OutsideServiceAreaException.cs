namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when a submitted delivery location is outside any supported service area.
/// </summary>
public sealed class OutsideServiceAreaException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="OutsideServiceAreaException"/> instance.
	/// </summary>
	public OutsideServiceAreaException()
		: base(ErrorInfoFactory.CreateOutsideServiceArea())
	{
	}
}