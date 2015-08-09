using System.Collections.Specialized;
using System.Web;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Models.DB;
using OWASP_2013_Demo.Web;

namespace given_that_i_make_a_user_authenication_request
{
	[TestClass]
	public class when_I_dont_apply_best_security_practices
	{
		private static UserProvider _userProvider;
		private static Mock<HttpResponseBase> _mockedHttpResponseBase;
		private static Mock<HttpRequestBase> _mockedHttpRequestBase;
		private static Mock<ISiteConfiguration> _mockedSiteConfiguration;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			_mockedSiteConfiguration = new Mock<ISiteConfiguration>();
			_mockedHttpResponseBase = new Mock<HttpResponseBase>();
			_mockedHttpRequestBase = new Mock<HttpRequestBase>();

			_mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("margaret0@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "margaret0@adventure-works.com",
					PasswordHash = "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=",
					PasswordSalt = "i2U3DxA=",
					FirstName = "Margaret",
					MiddleName = "J",
					LastName = "Smith",
					BusinessEntityID = 303,
					CustomerID = 29490,
					PersonID = 303,
					Title = "Ms."
				});

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("brenda18@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "brenda18@adventure-works.com",
					FirstName = "Brenda",
					LastName = "Garcia"
				});

			AutoMapperConfig.RegisterMapping();

			// In the future maybe mock PasswordManager too.
			_userProvider = new UserProvider(mockedUserRepository.Object, new PasswordManager(), 
				_mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void i_should_get_a_successful_login_response_with_a_user_object_using_valid_credentials()
		{
			var result = _userProvider.AuthenticateUser("margaret0@adventure-works.com", "password", _mockedHttpResponseBase.Object, false);
			result.UserPrincipal.Should().NotBeNull();
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

		[TestMethod]
		public void i_should_able_to_get_the_active_user_from_email_address_on_querystring()
		{
			const string emailAddress = "margaret0@adventure-works.com";

			// I don't like doing setup work in test methods but this seems to the the only way for quickness
			_mockedHttpRequestBase.Setup(x => x.QueryString)
				.Returns(new NameValueCollection {{"email", emailAddress}});

			var result = _userProvider.GetUserFromSelector(_mockedHttpRequestBase.Object, _mockedSiteConfiguration.Object);
			result.EmailAddress.Should().Be(emailAddress);
		}

		[TestMethod]
		public void i_should_able_to_get_the_active_user_from_email_address_on_querystring_for_another_user()
		{
			const string emailAddress = "brenda18@adventure-works.com";

			// I don't like doing setup work in test methods but this seems to the the only way for quickness
			_mockedHttpRequestBase.Setup(x => x.QueryString)
				.Returns(new NameValueCollection { { "email", emailAddress } });

			var result = _userProvider.GetUserFromSelector(_mockedHttpRequestBase.Object, _mockedSiteConfiguration.Object);
			result.EmailAddress.Should().Be(emailAddress);
		}
	}

	[TestClass]
	public class when_I_do_apply_best_security_practices
	{
		private static UserProvider _userProvider;
		private static Mock<HttpResponseBase> _mockedHttpResponseBase;
		private static Mock<HttpRequestBase> _mockedHttpRequestBase;
		private static Mock<ISiteConfiguration> _mockedSiteConfiguration;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedUserRepository = new Mock<IUserRepository>();
			var passwordManager = new PasswordManager();
			_mockedSiteConfiguration = new Mock<ISiteConfiguration>();
			_mockedHttpResponseBase = new Mock<HttpResponseBase>();
			_mockedHttpRequestBase = new Mock<HttpRequestBase>();

			_mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(true);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("nouser@madeup.com"))
				.Returns((IUser)null);

			mockedUserRepository.Setup(x => x.FetchUserByEmailAddress("margaret0@adventure-works.com"))
				.Returns(new User()
				{
					EmailAddress = "margaret0@adventure-works.com",
					PasswordHash = "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=",
					PasswordSalt = "i2U3DxA="
				});

			AutoMapperConfig.RegisterMapping();

			_userProvider = new UserProvider(mockedUserRepository.Object, passwordManager, _mockedSiteConfiguration.Object);
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
			result.UserPrincipal.EmailAddress.Should().Be(email);
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

		[TestMethod]
		public void i_should_not_able_to_get_information_for_another_user_using_their_email_address_on_querystring()
		{
			const string emailAddress = "brenda18@adventure-works.com";

			// I don't like doing setup work in test methods but this seems to the the only way for quickness
			_mockedHttpRequestBase.Setup(x => x.QueryString)
				.Returns(new NameValueCollection { { "email", emailAddress } });

			var result = _userProvider.GetUserFromSelector(_mockedHttpRequestBase.Object, _mockedSiteConfiguration.Object);

			// As I'm not mocking the users cookie null is expected for this test. As long as it is null or not the value in
			// emailAddress this is a successful test
			result.Should().BeNull();
		}
	}
}
