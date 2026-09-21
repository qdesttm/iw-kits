using IWKits.Core.Application.Modules.Tokens.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace IWKits.Api.AspNetCore.Modules.Tokens.IssueTokens;

[Mapper]
internal static partial class IssueTokensModelAdapter
{
	public static partial IssueTokensRequest ToIssueTokensRequest(this IssueTokensModel model);
}