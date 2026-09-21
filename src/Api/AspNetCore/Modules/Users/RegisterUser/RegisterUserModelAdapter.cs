using IWKits.Core.Application.Modules.Users.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace IWKits.Api.AspNetCore.Modules.Users.RegisterUser;

[Mapper]
internal static partial class RegisterUserModelAdapter
{
	public static partial RegisterUserRequest ToRegisterUserRequest(this RegisterUserModel model);
}