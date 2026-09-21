namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when user authentication fails due to incorrect credentials.
/// </summary>
public sealed class InvalidCredentialsException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="InvalidCredentialsException"/> instance.
	/// </summary>
	public InvalidCredentialsException()
		: base(ErrorInfoFactory.CreateInvalidCredentials())
	{
	}
}
