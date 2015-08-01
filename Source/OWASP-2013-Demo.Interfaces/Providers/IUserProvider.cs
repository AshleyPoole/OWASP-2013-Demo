using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IUserProvider
	{
		string NoUserExistsError { get; }
		string UserPasswordIncorrectError { get; }
		string UsernameOrPassworIncorrectError { get; }
		IUser AuthenticateUser(string emailAddress, string password);
	}
}
