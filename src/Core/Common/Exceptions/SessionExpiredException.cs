namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when the user session has expired.
/// </summary>
public sealed class SessionExpiredException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="SessionExpiredException"/> instance.
	/// </summary>
	public SessionExpiredException()
		: base(ErrorInfoFactory.CreateSessionExpired())
	{
	}
}