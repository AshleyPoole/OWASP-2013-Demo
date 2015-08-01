using System;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Interfaces.Utilities;

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

		public UserProvider(IUserRepository customerRepository, ISiteConfiguration siteConfiguration, IPasswordManager passwordManager)
		{
			_customerRepository = customerRepository;
			_siteConfiguration = siteConfiguration;
			_passwordManager = passwordManager;
		}

		public IAuthentication AuthenticateUser(string emailAddress, string password)
		{
			throw new NotImplementedException();
		}
	}
}
