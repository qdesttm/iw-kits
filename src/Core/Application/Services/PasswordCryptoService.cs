namespace IWKits.Core.Application.Services;

public sealed class PasswordCryptoService : IPasswordCryptoService
{
	public string GetHash(string password)
		=> BCrypt.Net.BCrypt.HashPassword(password);

	public bool Verify(string hashedPassword, string providedPassword)
		=> BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}