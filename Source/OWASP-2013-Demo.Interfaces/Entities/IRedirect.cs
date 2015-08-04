using System;

namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface IRedirect
	{
		bool Allowed { get; set; }
		Uri Url { get; set; }
		string ErrorMessage { get; set; }
	}
}
