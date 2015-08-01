using System.Web.Mvc;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class RedirectController : Controller
	{
		private readonly IRedirectProvider _redirectProvider;
		private readonly ISiteConfiguration _siteConfiguration;

		public RedirectController(IRedirectProvider redirectProvider, ISiteConfiguration siteConfiguration)
		{
			_redirectProvider = redirectProvider;
			_siteConfiguration = siteConfiguration;
		}

		// GET: Go
		[Route("Index/{url}")]
		public ActionResult Index(string url)
		{
			_siteConfiguration.UpdateSecureMode(Request);
			var viewModel = new BaseViewModel() { SecureMode = _siteConfiguration.SecureMode };

			var redirectResponse = _redirectProvider.ProcessRedirection(url);

			// Redirect client to requested url if allowed
			if (redirectResponse.Allowed) return Redirect(redirectResponse.Url.ToString());

			// Else return the error
			viewModel.ErrorText = redirectResponse.ErrorMessage;

			return View(viewModel);
		}
	}
}