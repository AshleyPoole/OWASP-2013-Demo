namespace OWASP_2013_Demo.Interfaces
{
	public interface ICustomerRepository
	{
		dynamic FetchUserByEmailAddress(string email);
	}
}
