using System.Configuration;
using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.Domain
{
	public class ConfigurationProvider : IConfigurationProvider
	{
		private string _allowedDomain;

		public string GetAllowedDomain()
		{
			return _allowedDomain ?? (_allowedDomain = ConfigurationManager.AppSettings["AllowedDomain"]);
		}
	}
}
