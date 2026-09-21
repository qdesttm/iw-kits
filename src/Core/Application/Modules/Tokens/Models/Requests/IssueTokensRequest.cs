using MediatR;

namespace IWKits.Core.Application.Modules.Tokens.Models.Requests;

/// <summary>
/// Represents a request to issue a new pair of security tokens using credentials.
/// </summary>
/// <param name="Username">The unique username of the user requesting authentication.</param>
/// <param name="Password">The plain-text password to be verified against the secure database hash.</param>
public sealed record IssueTokensRequest(string Username, string Password) : IRequest<TokensResponse>;