using MediatR;

namespace IWKits.Core.Application.Modules.Tokens.Models.Requests;

/// <summary>
/// Represents a request to refresh an existing authentication session using a valid refresh token.
/// </summary>
/// <param name="RefreshToken">The active cryptographic refresh token provided by the client.</param>
public sealed record RefreshTokensRequest(string RefreshToken) : IRequest<TokensResponse>;