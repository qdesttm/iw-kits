namespace IWKits.Core.Common;

/// <summary>
/// Provides API error codes.
/// </summary>
public static class ErrorCode
{
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
}