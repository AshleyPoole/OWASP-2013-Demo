using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
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

		public IUserPrincipal User
		{
			get
			{
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					// The user is authenticated. Return the user from the forms auth ticket.
					return ((MySecurityPrincipal)(HttpContext.Current.User)).User;
				}
				else if (HttpContext.Current.Items.Contains("User"))
				{
					// The user is not authenticated, but has successfully logged in.
					return (UserPrincipal)HttpContext.Current.Items["User"];
				}
				else
				{
					return null;
				}
			}
		}

		public static IUserPrincipal UserPrincipal
		{
			get
			{
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					// The user is authenticated. Return the user from the forms auth ticket.
					return ((MySecurityPrincipal)(HttpContext.Current.User)).User;
				}
				else if (HttpContext.Current.Items.Contains("User"))
				{
					// The user is not authenticated, but has successfully logged in.
					return (UserPrincipal)HttpContext.Current.Items["User"];
				}
				else
				{
					return null;
				}
			}
		}

		public UserProvider(IUserRepository customerRepository, IPasswordManager passwordManager, ISiteConfiguration siteConfiguration)
		{
			_customerRepository = customerRepository;
			_passwordManager = passwordManager;
			_siteConfiguration = siteConfiguration;
		}

		public IAuthentication AuthenticateUser(string emailAddress, string password, HttpResponseBase response)
		{
			var authResponse = new Authentication();
			var user = _customerRepository.FetchUserByEmailAddress(emailAddress);

			// ** TO DO. SANITISE USER INPUT

			if (user == null)
			{
				authResponse.Authenticated = false;
				authResponse.ErrorText = _siteConfiguration.SecureMode ? UsernameOrPassworIncorrectError : NoUserExistsError;

				return authResponse;
			}

			authResponse.User = user;
			authResponse.Authenticated = _passwordManager.PasswordMatchesHash(password, user.PasswordHash, user.PasswordSalt);

			if (authResponse.Authenticated)
			{
				var userData = new JavaScriptSerializer().Serialize(authResponse.User);

				var ticket = new FormsAuthenticationTicket(1,
						authResponse.User.EmailAddress,
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
			else
			{
				authResponse.ErrorText = _siteConfiguration.SecureMode ? UsernameOrPassworIncorrectError : UserPasswordIncorrectError;
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
	}
}
