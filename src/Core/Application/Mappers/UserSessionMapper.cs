using IWKits.Core.Common.Models;
using IWKits.Core.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace IWKits.Core.Application.Mappers;

[Mapper]
internal static partial class UserSessionMapper
{
	public static partial UserSession ToUserSession(this UserSessionEntity entity);

	public static partial UserSessionEntity ToUserSessionEntity(this UserSession userSession);
}