using System.ComponentModel.DataAnnotations;

namespace OWASP_2013_Demo.Web.Models
{
	public class RedirectRequest
	{
		[Required(ErrorMessage = "Url parameter is required for redirection. Please supply this parameter and try again.")]
		public string Url;

		public string Error;
	}
}