using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces;


namespace given_that_i_make_a_redirection_request
{
	[TestClass]
	public class when_I_dont_apply_best_security_pratices
	{
		private static GoRedirector _redirectProvider;
		private const string AnotherUrl = "http://www.ashleypoole.co.uk/about-ashley-poole";

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedConfigProvider = new Mock<IConfigurationProvider>();
			mockedConfigProvider.Setup(x => x.GetAllowedDomain()).Returns("supersecure.site");

			_redirectProvider = new GoRedirector(mockedConfigProvider.Object);
		}

		[TestMethod]
		public void then_the_redirection_should_be_allowed_to_another_domain()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.Allowed.Should().BeTrue();
		}

		[TestMethod]
		public void then_the_redirection_url_should_match_that_of_the_input_url()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.UrlForRedirect.ToString().Should().Be(AnotherUrl);
		}

		[TestMethod]
		public void then_the_redirection_should_not_contain_any_errors()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.ErrorMessage.Should().BeNullOrEmpty();
		}
	}

	[TestClass]
	public class when_I_apply_best_security_best_pratices
	{
		private static GoRedirector _redirectProvider;
		private const string AllowedUrl = "http://www.supersecure.site/Authentication/Login";
		private const string AnotherUrl = "http://www.ashleypoole.co.uk/about-ashley-poole";

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedConfigProvider = new Mock<IConfigurationProvider>();
			mockedConfigProvider.Setup(x => x.GetAllowedDomain()).Returns("supersecure.site");

			// True to indicate secure mode is to activated
			_redirectProvider = new GoRedirector(mockedConfigProvider.Object, true);
		}

		// Negative Tests
		[TestMethod]
		public void then_the_redirection_should_not_be_allowed_to_another_domain()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.Allowed.Should().BeFalse();
		}

		[TestMethod]
		public void then_the_redirection_url_should_be_null_for_another_domain_as_this_is_a_invalid_redirect()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.UrlForRedirect.Should().BeNull();
		}

		[TestMethod]
		public void then_the_redirection_should_contain_an_error_message_for_another_domain()
		{
			var result = _redirectProvider.ProcessGoDirection(AnotherUrl);
			result.ErrorMessage.Should().NotBeNullOrEmpty();
		}


		// Positive Tests
		[TestMethod]
		public void then_the_redirection_should_be_allowed_to_the_same_domain()
		{
			var result = _redirectProvider.ProcessGoDirection(AllowedUrl);
			result.Allowed.Should().BeTrue();
		}

		[TestMethod]
		public void then_the_redirection_url_shouldmatch_that_of_the_input_for_allowed_url()
		{
			var result = _redirectProvider.ProcessGoDirection(AllowedUrl);
			result.UrlForRedirect.ToString().Should().Be(AllowedUrl);
		}

		[TestMethod]
		public void then_the_redirection_should_not_contain_an_error_message_for_allowed_domain()
		{
			var result = _redirectProvider.ProcessGoDirection(AllowedUrl);
			result.ErrorMessage.Should().BeNullOrEmpty();
		}
	}
}
