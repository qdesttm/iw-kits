namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when there is a mismatch between the tax rate and location postal codes.
/// </summary>
public sealed class PostalCodeMismatchException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="PostalCodeMismatchException"/> instance with specified postal codes.
	/// </summary>
	/// <param name="rateZip">The numerical postal code from the tax rate data.</param>
	/// <param name="locationZip">The numerical postal code from the geographical location data.</param>
	public PostalCodeMismatchException(int rateZip, int locationZip)
		: base(ErrorInfoFactory.CreatePostalCodeMismatch(rateZip, locationZip))
	{
	}
}