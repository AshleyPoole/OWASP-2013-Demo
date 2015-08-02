using System;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Interfaces.Utilities;
using OWASP_2013_Demo.Models;

namespace OWASP_2013_Demo.Domain
{
	public class UserProvider : IUserProvider
	{
		private readonly IUserRepository _customerRepository;
		private readonly ISiteConfiguration _siteConfiguration;
		private readonly IPasswordManager _passwordManager;

		public string NoUserExistsError
		{
			get { return "Error: No user exists with that email address."; }
		}
		public string UserPasswordIncorrectError {
			get { return "Error: Password is incorrect. Please try again."; }
		}
		public string UsernameOrPassworIncorrectError {
			get { return "Error: Username or password is incorrect. Please try again."; }
		}

		public UserProvider(IUserRepository customerRepository, IPasswordManager passwordManager, ISiteConfiguration siteConfiguration)
		{
			_customerRepository = customerRepository;
			_passwordManager = passwordManager;
			_siteConfiguration = siteConfiguration;
		}

		public IAuthentication AuthenticateUser(string emailAddress, string password)
		{
			var authResponse = new Authentication();
			var user = _customerRepository.FetchUserByEmailAddress(emailAddress);

			// ** TO DO. SANITISE USER INPUT

			if (user == null)
			{
				authResponse.Authenticated = false;
				authResponse.ErrorText = !_siteConfiguration.SecureMode ? NoUserExistsError : UsernameOrPassworIncorrectError;
			}
			else
			{
				authResponse.User = user;
				authResponse.Authenticated = _passwordManager.PasswordMatchesHash(password, user.PasswordHash, user.PasswordSalt);
			}

			return authResponse;
		}
	}
}
