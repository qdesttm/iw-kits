namespace IWKits.Core.Common;

/// <summary>
/// Provides API error codes.
/// </summary>
public static class ErrorCode
{
	/// <summary>
	/// The error code representing a situation when the provided username or password during authentication is invalid.
	/// </summary>
	public const string InvalidCredentials = "invalid_credentials";

	/// <summary>
	/// The error code representing a situation when the provided username is already registered in the system.
	/// </summary>
	public const string UsernameAlreadyTaken = "username_already_taken";

	/// <summary>
	/// The error code representing a situation when the provided refresh token is not found or invalid.
	/// </summary>
	public const string SessionNotFound = "session_not_found";

	/// <summary>
	/// The error code representing a situation when the session has expired.
	/// </summary>
	public const string SessionExpired = "session_expired";

	/// <summary>
	/// The error code representing a situation when the requested user is not found.
	/// </summary>
	public const string UserNotFound = "user_not_found";

	/// <summary>
	/// The error code representing a situation when the ZIP codes of the provided tax rate and location do not match.
	/// </summary>
	public const string PostalCodeMismatch = "postal_code_mismatch";

	/// <summary>
	/// The error code representing a situation when the requested order delivery location is outside any supported service area.
	/// </summary>
	public const string OutsideServiceArea = "outside_service_area";

	/// <summary>
	/// The error code representing a situation when the specific geographic jurisdiction or city data cannot be resolved for the location.
	/// </summary>
	public const string JurisdictionNotResolved = "jurisdiction_not_resolved";

	/// <summary>
	/// The error code representing a situation when the required tax rate details are unavailable for the location.
	/// </summary>
	public const string TaxDataUnavailable = "tax_data_unavailable";

	/// <summary>
	/// The error code representing an unexpected system failure or unhandled operation exception.
	/// </summary>
	public const string OperationFailed = "operation_failed";
}