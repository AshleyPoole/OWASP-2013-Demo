using System;
using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.Domain
{
	class CustomerProvider : ICustomerProvider
	{
		public ICustomer GetCustomerByEmailAddress(string emailAddress)
		{
			throw new NotImplementedException();
		}
	}
}
