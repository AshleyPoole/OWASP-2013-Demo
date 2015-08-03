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

			var newHash = Convert.ToBase64String(CreateMD5Hash(password, salt));

			return existingHash == newHash;
		}

		private byte[] CreateMD5Hash(byte[] passwordBytes, byte[] saltBytes)
		{
			var md5Hasher = new HMACMD5(saltBytes);
			var saltedHash = md5Hasher.ComputeHash(passwordBytes);

			return saltedHash;
		}

		private byte[] CreateSHA1Hash(byte[] passwordBytes, byte[] saltBytes)
		{
			var sha1Hasher = new HMACSHA1(saltBytes);
			var saltedHash = sha1Hasher.ComputeHash(passwordBytes);

			return saltedHash;
		}
	}
}
