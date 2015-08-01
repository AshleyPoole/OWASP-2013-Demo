using System;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;

namespace OWASP_2013_Demo.Domain
{
	public class CustomerProvider : ICustomerProvider
	{
		private readonly ICustomerRepository _customerRepository;

		public CustomerProvider(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public ICustomer GetCustomerByEmailAddress(string emailAddress)
		{
			throw new NotImplementedException();
		}
	}
}
