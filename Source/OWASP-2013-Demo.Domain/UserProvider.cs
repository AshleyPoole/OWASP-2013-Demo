using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using AutoMapper;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Interfaces.Utilities;
using OWASP_2013_Demo.Models;
using OWASP_2013_Demo.Models.DB;

namespace OWASP_2013_Demo.Domain
{
	public class UserProvider : IUserProvider
	{
		private readonly IUserRepository _customerRepository;
		private readonly ISiteConfiguration _siteConfiguration;
		private readonly IPasswordManager _passwordManager;

		public string NoUserExistsError => "Error: No user exists with that email address.";

		public string UserPasswordIncorrectError => "Error: Password is incorrect. Please try again.";

		public string UsernameOrPasswordIncorrectError => "Error: Username or password is incorrect. Please try again.";

		public UserProvider(IUserRepository customerRepository, IPasswordManager passwordManager, ISiteConfiguration siteConfiguration)
		{
			_customerRepository = customerRepository;
			_passwordManager = passwordManager;
			_siteConfiguration = siteConfiguration;
		}

		public IAuthentication AuthenticateUser(string emailAddress, string password, HttpResponseBase response, bool addCookie)
		{
			var authResponse = new Authentication();
			var user = _customerRepository.FetchUserByEmailAddress(emailAddress);

			// ** TO DO. SANITISE USER INPUT

			if (user == null)
			{
				authResponse.Authenticated = false;
				authResponse.ErrorText = _siteConfiguration.SecureMode ? UsernameOrPasswordIncorrectError : NoUserExistsError;

				return authResponse;
			}

			// Converting user db to user prinicpal
			authResponse.UserPrincipal = Mapper.Map<UserPrincipal>(user);
			authResponse.Authenticated = _passwordManager.PasswordMatchesHash(password, user.PasswordHash, user.PasswordSalt);

			if (authResponse.Authenticated)
			{
				if (addCookie)
				{
					// Serialising the UserPrincipal object rather than user as user contains extra information relating to passwords
					var userData = new JavaScriptSerializer().Serialize(authResponse.UserPrincipal);

					var ticket = new FormsAuthenticationTicket(1,
							authResponse.UserPrincipal.EmailAddress,
							DateTime.Now,
							DateTime.Now.AddDays(30),
							true,
							userData,
							FormsAuthentication.FormsCookiePath);

					// Encrypt the ticket.
					var encTicket = FormsAuthentication.Encrypt(ticket);

					// Create the cookie.
					response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
				}
			}
			else
			{
				authResponse.ErrorText = _siteConfiguration.SecureMode ? UsernameOrPasswordIncorrectError : UserPasswordIncorrectError;
			}

			return authResponse;
		}

		public void Logoff(HttpSessionStateBase session, HttpResponseBase response)
		{
			// Delete the user details from cache.
			session.Abandon();

			// Delete the authentication ticket and sign out.
			FormsAuthentication.SignOut();

			// Clear authentication cookie.
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "") {Expires = DateTime.Now.AddYears(-1)};
			response.Cookies.Add(cookie);
		}

		private IUserPrincipal GetUserFromCookie(HttpRequestBase request)
		{
			UserPrincipal userData = null;

			try
			{
				var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
				var ticket = FormsAuthentication.Decrypt(authCookie.Value);
				userData = new JavaScriptSerializer().Deserialize<UserPrincipal>(ticket.UserData);
			}
			catch (Exception)
			{
				// This condition is expected if the cookie is missing. Simply allow null to be returned.
			}

			return userData;
		}

		private IUserPrincipal GetUserFromQueryString(HttpRequestBase request)
		{
			// This should be the same to grab session id or a business entity id
			var email = request.QueryString["email"];

			if (string.IsNullOrEmpty(email))
			{
				return null;
			}

			var userDb = _customerRepository.FetchUserByEmailAddress(email) ?? new User();
			var userPrincipal = Mapper.Map<UserPrincipal>(userDb);

			return userPrincipal;
		}

		public IUserPrincipal GetUserFromSelector(HttpRequestBase request, ISiteConfiguration siteConfiguration)
		{
			return siteConfiguration.SecureMode ? GetUserFromCookie(request) : GetUserFromQueryString(request);
		}
	}
}
