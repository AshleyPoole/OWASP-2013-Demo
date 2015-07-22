﻿using System;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Models;

namespace OWASP_2013_Demo.Domain
{
	public class RedirectProvider : IRedirectProvider
	{
		private readonly bool _secure;
		private readonly IConfigurationProvider _configurationProvider;

	    // Default to unsecure mode
		public RedirectProvider (IConfigurationProvider configurationProvider, bool secure = false)
		{
			_secure = secure;
			_configurationProvider = configurationProvider;
		}

		public Redirect ProcessGoDirection(string url)
		{
			var uri = new Uri(url);
			var redirectResponse = new Redirect() {Allowed = true, UrlForRedirect = uri};

			if (_secure)
			{
				// Secure mode activated
				var allowedDomain = _configurationProvider.GetAllowedDomain();

				if (!uri.Host.EndsWith(allowedDomain) || !uri.IsAbsoluteUri)
				{
					redirectResponse.Allowed = false;
					redirectResponse.UrlForRedirect = null;
					redirectResponse.ErrorMessage =
						string.Format(
							"Potentially dangerous redirect detected and blocked. Submitted url ({0}) did not match allowed domain list or was malformed.", url);
				}
			}

			return redirectResponse;
		}
	}
}