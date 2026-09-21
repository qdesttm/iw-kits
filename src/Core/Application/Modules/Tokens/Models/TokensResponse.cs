using IWKits.Core.Common.Messages;

namespace IWKits.Core.Application.Modules.Tokens.Models;

/// <summary>
/// Represents the API response containing a pair of security tokens.
/// </summary>
/// <param name="AccessToken">The JSON Web Token (JWT) used for resource authorization.</param>
/// <param name="RefreshToken">The secure token used to rotate and obtain a new access token.</param>
public sealed record TokensResponse(string AccessToken, string RefreshToken) : Response;