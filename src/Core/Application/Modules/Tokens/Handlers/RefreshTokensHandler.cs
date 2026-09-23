using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Modules.Tokens.Models;
using IWKits.Core.Application.Modules.Tokens.Models.Requests;
using IWKits.Core.Application.Services;
using MediatR;

namespace IWKits.Core.Application.Modules.Tokens.Handlers;

internal sealed class RefreshTokensHandler(
	IUserSessionService sessionService) :
	IRequestHandler<RefreshTokensRequest, TokensResponse>
{
	public async Task<TokensResponse> Handle(
		RefreshTokensRequest request,
		CancellationToken cancellationToken)
	{
		var sessionContext = await sessionService.RefreshSessionAsync(request.RefreshToken, cancellationToken);

		return new TokensResponse(
			sessionContext.AccessToken,
			sessionContext.Session.RefreshToken
		);
	}
}