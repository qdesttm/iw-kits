namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when required tax rate details are unavailable for an identified postal code.
/// </summary>
public sealed class TaxDataUnavailableException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="TaxDataUnavailableException"/> instance with a specified postal code.
	/// </summary>
	/// <param name="zipCode">The numerical postal or ZIP code.</param>
	public TaxDataUnavailableException(int zipCode)
		: base(ErrorInfoFactory.CreateTaxDataUnavailable(zipCode))
	{
	}
}