using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IUserProvider
	{
		IUser GetUserByEmailAddress(string emailAddress);

		

	}
}
