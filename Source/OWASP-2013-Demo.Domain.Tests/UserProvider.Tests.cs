using System.Web;
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
		private static Mock<HttpResponseBase> _mockedHttpResponseBase;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();
			_mockedHttpResponseBase = new Mock<HttpResponseBase>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("margaret0@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "margaret0@adventure-works.com",
					PasswordHash = "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=",
					PasswordSalt = "i2U3DxA="
				});

			// In the future maybe mock PasswordManager too.
			_userProvider = new UserProvider(mockedUserRepository.Object, new PasswordManager(), 
				mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_a_user_object_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.User.Should().NotBeNull();
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeTrue();
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void i_should_get_an_username_error_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "password", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().Be(_userProvider.NoUserExistsError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "password", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeFalse();
		}

		[TestMethod]
		public void i_should_get_an_password_error_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "superfalsesecret", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().Be(_userProvider.UserPasswordIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "superfalsesecret", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeFalse();
		}
	}

	[TestClass]
	public class when_I_do_apply_best_security_practices
	{
		private static UserProvider _userProvider;
		private static Mock<HttpResponseBase> _mockedHttpResponseBase;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();
			var passwordManager = new PasswordManager();
			_mockedHttpResponseBase = new Mock<HttpResponseBase>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(true);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("margaret0@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "margaret0@adventure-works.com",
					PasswordHash = "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=",
					PasswordSalt = "i2U3DxA="
				});

			_userProvider = new UserProvider(mockedUserRepository.Object, passwordManager, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeTrue();
		}

		[TestMethod]
		public void i_should_get_a_correctly_populated_user_object_with_valid_credentials()
		{
			var email = "margaret0@adventure-works.com";
			var result = _userProvider.AuthenticateUser(email, "supertruesecret", _mockedHttpResponseBase.Object, false);
			result.User.EmailAddress.Should().Be(email);
		}

		[TestMethod]
		public void i_should_not_get_any_errors_for_a_successful_login_response_with_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "password", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().Be(_userProvider.UsernameOrPasswordIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_email_address_does_not_exist()
		{
			var result = _userProvider.AuthenticateUser("nouser@madeup.com", "password", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeFalse();
		}

		[TestMethod]
		public void i_should_get_an_generic_error_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "superfalsesecret", _mockedHttpResponseBase.Object, false);
			result.ErrorText.Should().Be(_userProvider.UsernameOrPasswordIncorrectError);
		}

		[TestMethod]
		public void i_should_get_an_authenication_failure_if_login_password_is_incorrect()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "superfalsesecret", _mockedHttpResponseBase.Object, false);
			result.Authenticated.Should().BeFalse();
		}
	}
}
