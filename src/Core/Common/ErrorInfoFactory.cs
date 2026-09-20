using System;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Common;

/// <summary>
/// Represents a factory to create error information instances.
/// </summary>
public static class ErrorInfoFactory
{
	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the provided refresh token was not found or is invalid.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateSessionNotFound()
		=> new(
			ErrorCode.SessionNotFound,
			"The requested session was not found or the refresh token is invalid.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the session has expired.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateSessionExpired()
		=> new(
			ErrorCode.SessionExpired,
			"The session has expired. Please log in again.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the user associated with the session no longer exists.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateUserNotFound(Guid userId)
		=> new(
			ErrorCode.UserNotFound,
			$"The user with ID '{userId}' associated with this session was not found.");
}