using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OWASP_2013_Demo.Authentication.Encryption;

namespace OWASP_2013_Demo.Authentication
{
	public interface IAuthenticationService
	{
		void RegisterNewUser(string username, string password);

		bool IsPasswordCorrectForUser(string username, string password);

		bool IsUsernameValid(string username);

		bool IsUsernameTaken(string username);

		bool IsPasswordValid(string password);
	}

	public class ExampleAuthenticationService : IAuthenticationService
	{
		private readonly IAuthenticationDatabase authenticationDatabase;
		private readonly IPasswordValidator passwordValidator;

		public ExampleAuthenticationService(IAuthenticationDatabase authenticationDatabase, IPasswordValidator passwordValidator)
		{
			this.authenticationDatabase = authenticationDatabase;
			this.passwordValidator = passwordValidator;
		}

		// Checks if the user's password is correct
		public bool IsPasswordCorrectForUser(string username, string password)
		{
			var credentials = authenticationDatabase.GetUserCredentialsByUsername(username);
			return credentials != null && passwordValidator.IsPasswordCorrect(password, credentials.PasswordValidationString);
		}

		// Registers a new user with the given username and password
		public void RegisterNewUser(string username, string password)
		{
			// Check validity
			if (IsUsernameValid(username) && !IsUsernameTaken(username) && IsPasswordValid(password))
			{
				authenticationDatabase.SaveUserCredentials(new UserCredentials()
				{
					Username = username,
					PasswordValidationString = passwordValidator.GenerateValidationString(password)
				});
			}
			else
			{
				throw new ArgumentException();
			}
		}

		// Utility function to encrypt user passwords
		public string GenerateValidationString(string password)
		{
			return password;
		}

		public bool CheckPasswordAgainstValidationString(string password, string validationString)
		{
			return password == validationString;
		}

		public bool IsUsernameValid(string username)
		{
			return Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
		}

		public bool IsUsernameTaken(string username)
		{
			return authenticationDatabase.GetUserCredentialsByUsername(username) != null;
		}

		public bool IsPasswordValid(string password)
		{
			return password.Length >= 6;
		}
	}
}
