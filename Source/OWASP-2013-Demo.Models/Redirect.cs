using System;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Models
{
	public class Redirect : IRedirect
	{
		public bool Allowed { get; set; }
		public Uri Url { get; set; }
		public string ErrorMessage { get; set; }

		public Redirect()
		{
			Allowed = false;
		}
	}
}
