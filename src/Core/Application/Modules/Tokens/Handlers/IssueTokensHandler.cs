using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Modules.Tokens.Models;
using IWKits.Core.Application.Modules.Tokens.Models.Requests;
using IWKits.Core.Application.Services;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MediatR;
using MongoDB.Driver;

namespace IWKits.Core.Application.Modules.Tokens.Handlers;

internal sealed class IssueTokensHandler(
	MongoDbContext dbContext,
	IPasswordCryptoService cryptoService,
	IUserSessionService sessionService) :
	IRequestHandler<IssueTokensRequest, TokensResponse>
{
	public async Task<TokensResponse> Handle(
		IssueTokensRequest request,
		CancellationToken cancellationToken)
	{
		var filter = Builders<UserEntity>.Filter.Eq(x => x.Username, request.Username);

		var userEntity = await dbContext.Users
				.Find(filter)
				.FirstOrDefaultAsync(cancellationToken)
			?? throw new InvalidCredentialsException();

		if (!cryptoService.Verify(userEntity.PasswordHash, request.Password))
			throw new InvalidCredentialsException();

		var user = userEntity.ToUser();

		var sessionContext = await sessionService.CreateSessionAsync(user, cancellationToken);

		return new TokensResponse(
			sessionContext.AccessToken,
			sessionContext.Session.RefreshToken
		);
	}
}