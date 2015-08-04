namespace OWASP_2013_Demo.Interfaces.Utilities
{
	public interface IPasswordManager
	{
		bool PasswordMatchesHash(string unencryptedPassword, string passwordHash, string passwordSalt);
	}
}
