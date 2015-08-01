namespace OWASP_2013_Demo.Interfaces
{
	public interface ICustomerProvider
	{
		ICustomer GetCustomerByEmailAddress(string emailAddress);
	}
}
