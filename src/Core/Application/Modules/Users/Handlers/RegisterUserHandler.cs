using System;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Modules.Users.Models.Requests;
using IWKits.Core.Application.Services;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MediatR;
using MongoDB.Driver;

namespace IWKits.Core.Application.Modules.Users.Handlers;

internal sealed class RegisterUserHandler(
	MongoDbContext dbContext,
	IPasswordCryptoService cryptoService) :
	IRequestHandler<RegisterUserRequest>
{
	private const string DefaultUserRole = "admin";

	public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
	{
		var filter = Builders<UserEntity>.Filter.Eq(x => x.Username, request.Username);
		var userExists = await dbContext.Users.Find(filter).AnyAsync(cancellationToken);

		if (userExists)
			throw new UsernameAlreadyTakenException();

		var passwordHash = cryptoService.GetHash(request.Password);

		var userEntity = new UserEntity
		{
			Id = Guid.NewGuid(),
			Username = request.Username,
			PasswordHash = passwordHash,
			Role = DefaultUserRole
		};

		await dbContext.Users.InsertOneAsync(userEntity, null, cancellationToken);
	}
}