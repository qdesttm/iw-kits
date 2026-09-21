namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when specific geographic jurisdiction data cannot be resolved for a state.
/// </summary>
public sealed class JurisdictionNotResolvedException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="JurisdictionNotResolvedException"/> instance with a specified state identifier.
	/// </summary>
	/// <param name="stateId">The identifier of the state where resolution failed.</param>
	public JurisdictionNotResolvedException(string stateId)
		: base(ErrorInfoFactory.CreateJurisdictionNotResolved(stateId))
	{
	}
}
