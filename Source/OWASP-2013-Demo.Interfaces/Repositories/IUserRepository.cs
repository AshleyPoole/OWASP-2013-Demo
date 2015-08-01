using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Repositories
{
	public interface IUserRepository
	{
		IUser FetchUserByEmailAddress(string email);
	}
}
