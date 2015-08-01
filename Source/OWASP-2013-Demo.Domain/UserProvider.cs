using System;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;

namespace OWASP_2013_Demo.Domain
{
	public class UserProvider : IUserProvider
	{
		private readonly IUserRepository _customerRepository;

		public UserProvider(IUserRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public IUser GetUserByEmailAddress(string emailAddress)
		{
			throw new NotImplementedException();
		}
	}
}
