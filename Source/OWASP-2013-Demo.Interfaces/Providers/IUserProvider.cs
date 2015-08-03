using System.Web;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IUserProvider
	{
		string NoUserExistsError { get; }
		string UserPasswordIncorrectError { get; }
		string UsernameOrPassworIncorrectError { get; }
		IUserPrincipal User { get; }
		IAuthentication AuthenticateUser(string emailAddress, string password, HttpResponseBase response);
		void Logoff(HttpSessionStateBase session, HttpResponseBase response);
	}
}
