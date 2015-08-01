using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Repositories;

namespace given_that_i_request_a_customer
{
	class when_I_suppy_a_valid_email_address
	{
		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedCustomerRepository = new Mock<ICustomerRepository>();

			mockedCustomerRepository.Setup(x => x.FetchUserByEmailAddress("git@ashleypoole.co.uk"))
				.Returns(new { Test = "test" });

			var customerProvider = new CustomerProvider(mockedCustomerRepository.Object);
		}
	}
}
