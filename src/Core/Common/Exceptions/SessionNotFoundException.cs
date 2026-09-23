namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when a requested session is not found or the refresh token is invalid.
/// </summary>
public sealed class SessionNotFoundException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="SessionNotFoundException"/> instance.
	/// </summary>
	public SessionNotFoundException()
		: base(ErrorInfoFactory.CreateSessionNotFound())
	{
	}
}