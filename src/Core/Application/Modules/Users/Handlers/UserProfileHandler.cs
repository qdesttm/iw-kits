using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Modules.Users.Models;
using IWKits.Core.Application.Modules.Users.Models.Requests;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MediatR;
using MongoDB.Driver;

namespace IWKits.Core.Application.Modules.Users.Handlers;

internal sealed class UserProfileHandler(
	MongoDbContext dbContext) :
	IRequestHandler<UserProfileRequest, UserProfileResponse>
{
	public async Task<UserProfileResponse> Handle(
		UserProfileRequest request,
		CancellationToken cancellationToken)
	{
		var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, request.UserId);

		var userEntity = await dbContext.Users
				.Find(filter)
				.FirstOrDefaultAsync(cancellationToken)
			?? throw new UserNotFoundException(request.UserId);

		var profile = userEntity.ToUserProfile();

		return new UserProfileResponse(profile);
	}
}