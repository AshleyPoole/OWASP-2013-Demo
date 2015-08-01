using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Models.DB;

namespace given_that_i_make_a_user_authenication_request
{
	[TestClass]
	public class when_I_dont_apply_best_security_practices
	{
		private static UserProvider _userProvider;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("john37@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "user@madeup.com",
					PasswordHash = "LwOT3SHzNXMVrmIIiF+0yex9Tp7tIoErVpYBktc6rAI=",
					PasswordSalt = "Zsrf+go="
				});

			_userProvider = new UserProvider(mockedUserRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_a_user_object_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "supertruesecret");
			result.User.Should().NotBeNull();
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "supertruesecret");
			result.Authenticated.Should().BeTrue();
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "supertruesecret");
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void i_should_get_an_username_error_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "supertruesecret");
			result.ErrorText.Should().Be(_userProvider.NoUserExistsError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "supertruesecret");
			result.Authenticated.Should().BeFalse();
		}

		[TestMethod]
		public void i_should_get_an_password_error_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "superfalsesecret");
			result.ErrorText.Should().Be(_userProvider.UserPasswordIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "superfalsesecret");
			result.Authenticated.Should().BeFalse();
		}
	}

	[TestClass]
	public class when_I_do_apply_best_security_practices
	{
		private static UserProvider _userProvider;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("john37@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "user@madeup.com",
					PasswordHash = "LwOT3SHzNXMVrmIIiF+0yex9Tp7tIoErVpYBktc6rAI=",
					PasswordSalt = "Zsrf+go="
				});

			_userProvider = new UserProvider(mockedUserRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "supertruesecret");
			result.Authenticated.Should().BeTrue();
		}

		[TestMethod]
		public void i_should_get_a_correctly_populated_user_object_with_valid_credentials()
		{
			var email = "john37@adventure-works.com";
			var result = _userProvider.AuthenticateUser(email, "supertruesecret");
			result.User.EmailAddress.Should().Be(email);
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_with_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "supertruesecret");
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "supertruesecret");
			result.ErrorText.Should().Be(_userProvider.UsernameOrPassworIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "supertruesecret");
			result.Authenticated.Should().BeFalse();
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "superfalsesecret");
			result.ErrorText.Should().Be(_userProvider.UsernameOrPassworIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("john37@adventure-works.com", "superfalsesecret");
			result.Authenticated.Should().BeFalse();
		}
	}
}
