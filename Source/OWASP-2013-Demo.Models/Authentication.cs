﻿using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Models
{
	public class Authentication : IAuthentication
	{
		public IUserPrincipal UserPrincipal { get; set; }
		public bool Authenticated { get; set; }
		public string ErrorText { get; set; }

		public Authentication()
		{
			Authenticated = false;
		}
	}
}
