using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces.Utilities;

namespace given_that_i_compare_passwords
{
	[TestClass]
	public class when_a_user_tries_to_log_in
	{
		private static IPasswordManager _passwordManager;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			_passwordManager = new PasswordManager();
		}

		[TestMethod]
		public void then_true_should_be_returned_if_the_passwords_match()
		{
			var result = _passwordManager.PasswordMatchesHash("password", "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=", "i2U3DxA=");
			result.Should().BeTrue();
		}

		[TestMethod]
		public void then_false_should_be_returned_if_the_passwords_do_not_match()
		{
			var result = _passwordManager.PasswordMatchesHash("wrongpassword", "HyeF+GbkROa/eaUyYgVqCm8zMdNn/AEIzOnd+luTsgQ=", "i2U3DxA=");
			result.Should().BeFalse();
		}
	}
}
