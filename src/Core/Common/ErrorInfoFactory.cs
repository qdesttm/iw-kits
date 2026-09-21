using System;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Common;

/// <summary>
/// Represents a factory to create error information instances.
/// </summary>
public static class ErrorInfoFactory
{
	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the authentication credentials are invalid.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateInvalidCredentials()
		=> new(
			ErrorCode.InvalidCredentials,
			"The provided username or password is incorrect.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the requested username is already taken.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateUsernameAlreadyTaken()
		=> new(
			ErrorCode.UsernameAlreadyTaken,
			"This username is already taken by another account.");

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

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the requested delivery location is outside any supported service area.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateOutsideServiceArea()
		=> new(
			ErrorCode.OutsideServiceArea,
			"The selected delivery location is outside the supported service areas.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the specific geographic jurisdiction cannot be resolved for a state.
	/// </summary>
	/// <param name="stateId">The identifier of the state.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateJurisdictionNotResolved(string stateId)
		=> new(
			ErrorCode.JurisdictionNotResolved,
			$"Unable to calculate or resolve specific jurisdiction metadata for the selected location in state '{stateId}'.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the required tax rate details are unavailable.
	/// </summary>
	/// <param name="zipCode">The numerical postal or ZIP code.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateTaxDataUnavailable(int zipCode)
		=> new(
			ErrorCode.TaxDataUnavailable,
			$"Tax rate details are currently unavailable for the identified area with zip code '{zipCode}'.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance which indicates that the uploaded file is empty or missing.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateInvalidFile()
		=> new(
			ErrorCode.InvalidFile,
			"The uploaded file is empty, missing, or corrupted.");

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance encapsulating an unhandled system exception message.
	/// </summary>
	/// <param name="message">The raw technical or system exception message.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateOperationFailed(string message)
		=> new(
			ErrorCode.OperationFailed,
			message);

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance representing a validation or bad HTTP request failure.
	/// </summary>
	/// <param name="message">The descriptive message explaining what is wrong with the request input.</param>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateBadRequest(string message)
		=> new(ErrorCode.BadRequest, message);

	/// <summary>
	/// Creates a new <see cref="ErrorInfo"/> instance representing an unexpected internal server failure.
	/// </summary>
	/// <returns>A new <see cref="ErrorInfo"/> instance.</returns>
	public static ErrorInfo CreateInternalServerError()
		=> new(ErrorCode.InternalServerError, "An unexpected error occurred on the server.");
}