using System;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Models;

namespace OWASP_2013_Demo.Domain
{
	public class RedirectProvider : IRedirectProvider
	{
		private readonly ISiteConfiguration _siteConfiguration;

		public RedirectProvider (ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}

		public IRedirectObject ProcessGoDirection(string url)
		{
			var redirectResponse = new Redirect();

			// Check that we've received the url parameter
			if (string.IsNullOrEmpty(url))
			{
				redirectResponse.ErrorMessage = string.Format("Url parameter was missing or malformed - ({0}).", url);
				return redirectResponse;
			}

			// Check that url is valid as we don't want a broken redirect
			var uri = new Uri(url);
			redirectResponse.Allowed = true;
			redirectResponse.Url = uri;

			if (_siteConfiguration.SecureMode)
			{
				// Secure mode activated
				if (!uri.Host.EndsWith(_siteConfiguration.WebsiteDomain) || !uri.IsAbsoluteUri)
				{
					redirectResponse.Allowed = false;
					redirectResponse.Url = null;
					redirectResponse.ErrorMessage =
						string.Format(
							"Potentially dangerous redirect detected and blocked. Submitted url ({0}) did not match allowed domain list or was malformed.",
							url);
				}
			}

			return redirectResponse;
		}
	}
}
