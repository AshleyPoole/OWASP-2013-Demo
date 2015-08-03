using System.Web.Mvc;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly IUserProvider _userProvider;
		private readonly ISiteConfiguration _siteConfiguration;

		public AuthenticationController(IUserProvider userProvider, ISiteConfiguration siteConfiguration)
		{
			_userProvider = userProvider;
			_siteConfiguration = siteConfiguration;
		}

		// GET: Authentication
		[AllowAnonymous]
		//[Route("Index/")]
		public ActionResult Index()
		{
			_siteConfiguration.UpdateSecureMode(Request);
			var viewModel = new LoginViewModel() {SecureMode = _siteConfiguration.SecureMode};
			return View("Index", viewModel);
		}

		// GET: Authentication
		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(LoginViewModel model)
		{
			_siteConfiguration.UpdateSecureMode(Request);
			model.SecureMode = _siteConfiguration.SecureMode;

			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			var loginResult = _userProvider.AuthenticateUser(model.Email, model.Password, Response);

			if (!loginResult.Authenticated)
			{

				ModelState.AddModelError("", "Invalid login attempt.");
				return View("Index", model);
			}

			return RedirectToAction("Index", "Home");
		}

		public ActionResult Logout()
		{
			_siteConfiguration.UpdateSecureMode(Request);

			// Clear the user session and forms auth ticket.
			_userProvider.Logoff(Session, Response);

			return RedirectToAction("Index", "Home");
		}
	}
}