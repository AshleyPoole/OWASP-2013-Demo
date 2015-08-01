using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Models.DB;

namespace given_that_i_make_a_user_authenication_request
{
	class when_I_dont_apply_best_security_practices
	{
		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			// ** TO DO - COMPLETE THE RESPONSE IS THIS MOCK
			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("git@ashleypoole.co.uk"))
				.Returns(new User() {EmailAddress = "git@ashleypoole.co.uk"});

			// ** TO DO - ADD MORE SETUPS

			var customerProvider = new UserProvider(mockedUserRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_valid_credentials()
		{
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_with_valid_credentials()
		{
		}

		[TestMethod]
		public void i_should_get_an_username_error_if_login_email_address_does_not_exist()
		{
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
		}

		[TestMethod]
		public void i_should_get_an_password_error_if_login_password_is_incorrect()
		{
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
		}
	}
	class when_I_do_apply_best_security_practices
	{
		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			// ** TO DO - COMPLETE THE RESPONSE IS THIS MOCK
			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("git@ashleypoole.co.uk"))
				.Returns(new User() { EmailAddress = "git@ashleypoole.co.uk" });

			// ** TO DO - ADD MORE SETUPS

			var customerProvider = new UserProvider(mockedUserRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_valid_credentials()
		{
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_with_valid_credentials()
		{
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_email_address_does_not_exist()
		{
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_password_is_incorrect()
		{
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
		}
	}
}
