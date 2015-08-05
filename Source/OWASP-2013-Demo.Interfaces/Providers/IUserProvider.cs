using System.Web;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IUserProvider
	{
		string NoUserExistsError { get; }
		string UserPasswordIncorrectError { get; }
		string UsernameOrPasswordIncorrectError { get; }
		IAuthentication AuthenticateUser(string emailAddress, string password, HttpResponseBase response, bool addCookie);
		void Logoff(HttpSessionStateBase session, HttpResponseBase response);
		IUserPrincipal GetUserFromCookie(HttpRequestBase request);
		IUserPrincipal GetUserFromQueryString(HttpRequestBase request);
		IUserPrincipal GetUserFromSelector(HttpRequestBase request, ISiteConfiguration siteConfiguration);
	}
}
