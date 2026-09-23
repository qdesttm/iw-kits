namespace IWKits.Core.Application.Services;

public interface IPasswordCryptoService
{
	string GetHash(string password);

	bool Verify(string hashedPassword, string providedPassword);
}
