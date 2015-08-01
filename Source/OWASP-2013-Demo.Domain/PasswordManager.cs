using OWASP_2013_Demo.Interfaces.Utilities;

namespace OWASP_2013_Demo.Domain
{
	public class PasswordManager : IPasswordManager
	{
		public bool PasswordMatchesHashed(string unencryptedPassword, string passwordHash, string passwordSalt)
		{
			throw new System.NotImplementedException();
		}
	}
}
