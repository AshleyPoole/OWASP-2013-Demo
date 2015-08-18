using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWASP_2013_Demo.Authentication.Encryption
{
	public interface IPasswordValidator
	{
		string GenerateValidationString(string password);

		bool IsPasswordCorrect(string password, string validationString);
	}
}
