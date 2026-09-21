namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when a user registration fails because the username already exists.
/// </summary>
public sealed class UsernameAlreadyTakenException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="UsernameAlreadyTakenException"/> instance.
	/// </summary>
	public UsernameAlreadyTakenException()
		: base(ErrorInfoFactory.CreateUsernameAlreadyTaken())
	{
	}
}