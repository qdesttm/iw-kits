namespace IWKits.Api.AspNetCore.Modules.Tokens.IssueTokens;

internal sealed class IssueTokensModel
{
	public required string Username { get; init; }
	public required string Password { get; init; }
}