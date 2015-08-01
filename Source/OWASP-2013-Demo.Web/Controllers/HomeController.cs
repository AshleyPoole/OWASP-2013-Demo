using System.Web.Mvc;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISiteConfiguration _siteConfiguration;

		public HomeController(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}

		// GET: Home
		public ActionResult Index()
		{
			_siteConfiguration.UpdateSecureMode(Request);
			var viewModel = new BaseViewModel() { SecureMode = _siteConfiguration.SecureMode };

			return View(viewModel);
		}
	}
}