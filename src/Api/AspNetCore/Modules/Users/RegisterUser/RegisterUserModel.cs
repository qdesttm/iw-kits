namespace IWKits.Api.AspNetCore.Modules.Users.RegisterUser;

internal sealed class RegisterUserModel
{
	public required string Username { get; init; }
	public required string Password { get; init; }
}