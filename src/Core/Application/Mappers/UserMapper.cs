using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class UserMapper
{
	[MapperIgnoreSource(nameof(UserEntity.PasswordHash))]
	public static partial UserProfile ToUserProfile(this UserEntity entity);

	public static partial User ToUser(this UserEntity entity);

	public static partial UserEntity ToUserEntity(this User user);
}