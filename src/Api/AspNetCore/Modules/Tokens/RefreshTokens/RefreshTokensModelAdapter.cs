using IWKits.Core.Application.Modules.Tokens.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace IWKits.Api.AspNetCore.Modules.Tokens.RefreshTokens;

[Mapper]
internal static partial class RefreshTokensModelAdapter
{
	public static partial RefreshTokensRequest ToRefreshTokensRequest(this RefreshTokensModel model);
}