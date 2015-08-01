using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface ICustomerProvider
	{
		ICustomer GetCustomerByEmailAddress(string emailAddress);

		

	}
}
