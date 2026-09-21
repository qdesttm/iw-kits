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
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the requested user was not found.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateUserNotFound(Guid userId)
		=> new(
			ErrorCode.UserNotFound,
			$"The user with ID '{userId}' was not found.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that there is a mismatch between the tax rate and postal code.
	/// </summary>
	/// <param name="rateZip">The numerical postal code from the tax rate data.</param>
	/// <param name="locationZip">The numerical postal code from the geographical location data.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreatePostalCodeMismatch(int rateZip, int locationZip)
		=> new(
			ErrorCode.PostalCodeMismatch,
			$"Postal codes do not match: TaxRate has '{rateZip}' while Location has '{locationZip}'.");
}