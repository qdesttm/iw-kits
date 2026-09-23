using System;

namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when a requested user is not found.
/// </summary>
public sealed class UserNotFoundException : ServiceException
{
	/// <summary>
	/// Initializes a new <see cref="UserNotFoundException"/> instance with a specified user identifier.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	public UserNotFoundException(Guid userId)
		: base(ErrorInfoFactory.CreateUserNotFound(userId))
	{
	}
}