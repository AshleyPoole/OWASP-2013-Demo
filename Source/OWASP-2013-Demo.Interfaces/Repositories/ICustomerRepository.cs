namespace OWASP_2013_Demo.Interfaces.Repositories
{
	public interface ICustomerRepository
	{
		dynamic FetchUserByEmailAddress(string email);
	}
}
