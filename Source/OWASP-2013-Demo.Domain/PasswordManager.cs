using System;
using System.Security.Cryptography;
using OWASP_2013_Demo.Interfaces.Utilities;

namespace OWASP_2013_Demo.Domain
{
	public class PasswordManager : IPasswordManager
	{
		public bool PasswordMatchesHash(string unencryptedPassword, string existingHash, string passwordSalt)
		{
			var password = System.Text.Encoding.UTF8.GetBytes(unencryptedPassword);
			var salt = System.Text.Encoding.UTF8.GetBytes(passwordSalt);

			var newHash = Convert.ToBase64String(CreateSHA256Hash(unencryptedPassword, passwordSalt));

			return existingHash == newHash;
		}

		private byte[] CreateSHA256Hash(string password, string salt)
		{
			var password2 = System.Text.Encoding.UTF8.GetBytes(password);
			var salt2 = System.Text.Encoding.UTF8.GetBytes(salt);

			var md5Hasher = new HMACSHA256(salt2);
			var saltedHash = md5Hasher.ComputeHash(password2);

			return saltedHash;
		}
	}
}
