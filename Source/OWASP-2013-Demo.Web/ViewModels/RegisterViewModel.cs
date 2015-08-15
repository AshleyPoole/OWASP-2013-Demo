using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OWASP_2013_Demo.Web.ViewModels
{
	public class RegisterViewModel : BaseViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string VerifyPassword { get; set; }
	}
}