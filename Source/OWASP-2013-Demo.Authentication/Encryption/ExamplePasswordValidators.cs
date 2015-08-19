using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OWASP_2013_Demo.Authentication.Encryption
{
	// Not Secure
	public class PlaintextPasswordValidator : IPasswordValidator
	{
		public string GenerateValidationString(string password)
		{
			return password;
		}

		public bool IsPasswordCorrect(string password, string validationString)
		{
			return password == validationString;
		}
	}

	// Not Secure
	internal class EncryptedPasswordValidator : IPasswordValidator
	{
		// The attacker will find your encryption key, and then the whole database is plaintext.

		public string GenerateValidationString(string password)
		{
			var key = ConfigurationManager.AppSettings["SecretEncryptionKey"];
			// TODO: Return encrypted password
			throw new NotImplementedException();
		}

		public bool IsPasswordCorrect(string password, string validationString)
		{
			var key = ConfigurationManager.AppSettings["SecretEncryptionKey"];
			// TODO: 1. Decrypt Password
			// TODO: 2. Compare password to decrypted password
			throw new NotImplementedException();
		}
	}

	// Still not secure
	public class SimpleHashPasswordValidator : IPasswordValidator
	{
		// The whole database will be cracked using pre-computed rainbow tables

		public string GenerateValidationString(string password)
		{
			var sha = new SHA256Managed();

			// Password hashed directly = vulnerable to rainbow table attacks
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
			var validationString = Convert.ToBase64String(hash);

			return validationString;
		}

		public bool IsPasswordCorrect(string password, string validationString)
		{
			return validationString == GenerateValidationString(password);
		}
	}

	// Better, but still not secure
	internal class RandomSaltHashPasswordValidator : IPasswordValidator
	{
		// Still, Consumer GPUs can compute billions of NTLM, MD5, and SHA hashes per second.
		// This is a little more secure than storing a single salt because a rainbow table can not be computed
		private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

		public string GenerateValidationString(string password)
		{
			// generate a random salt to be stored with the hash of the password
			var salt = new byte[64]; 
			rng.GetBytes(salt);

			// TODO: Use completely random salt to compute SHA hash
			throw new NotImplementedException();
		}

		public bool IsPasswordCorrect(string password, string validationString)
		{
			// TODO: split validation string into salt and hash
			// TODO: then compute hash & compare
			throw new NotImplementedException();
		}
	}

	// Most secure
	public class PBKDF2PasswordValidator : IPasswordValidator
	{
		// This uses the C# standard verified implementation of PBKDF2 (RFC 2898)

		// Iterations are adjusted upwards as hardware becomes more powerful
		private const int ITERATIONS = 105000;

		private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

		public string GenerateValidationString(string password)
		{
			// generate a random salt to be stored with the hash of the password
			var salt = new byte[64];
			rng.GetBytes(salt);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
			var hashResult = pbkdf2.GetBytes(64);

			return string.Join(",", ITERATIONS, Convert.ToBase64String(salt), Convert.ToBase64String(hashResult));
		}

		public bool IsPasswordCorrect(string password, string validationString)
		{
			var fields = validationString.Split(',');
			int iterationsUsed = int.Parse(fields[0]);
			byte[] salt = Convert.FromBase64String(fields[1]);
			byte[] expectedResult = Convert.FromBase64String(fields[2]);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterationsUsed);
			var actualResult = pbkdf2.GetBytes(expectedResult.Length);

			return actualResult.SequenceEqual(expectedResult);
		}
	}
}
