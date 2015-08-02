using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Utilities;

namespace given_that_i_compare_passwords
{
	[TestClass]
	public class when_a_user_tries_to_log_in
	{
		private IPasswordManager _passwordManager;

		[ClassInitialize]
		public void Setup(TestContext testContext)
		{
			_passwordManager = new PasswordManager();
		}

		[TestMethod]
		public void then_true_should_be_returned_if_the_passwords_match()
		{
			var result = _passwordManager.PasswordMatchesHash("password", "37652c0d346fae62753b5e89ba857df1", "bE3XiWw=");
			result.Should().BeFalse();
		}

		[TestMethod]
		public void then_false_should_be_returned_if_the_passwords_do_not_match()
		{
			var result = _passwordManager.PasswordMatchesHash("incorrectpassword", "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
				"bE3XiWw=");
			result.Should().BeFalse();
		}

	}
}
