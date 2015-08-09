using System.Web.Mvc;
using AutoMapper;
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
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Index()
		{
			_siteConfiguration.UpdateSecureMode(Request);
			
			if (Request.IsAuthenticated)
			{
				var user = _userProvider.GetUserFromSelector(Request, _siteConfiguration);
				var loggedInViewModel = Mapper.Map<UserViewModel>(user);

				loggedInViewModel.SecureMode = _siteConfiguration.SecureMode;

				return RedirectToAction("Manage", new { email = user.EmailAddress });

			}

			var viewModel = new LoginViewModel() { SecureMode = _siteConfiguration.SecureMode };

			return View("Index", viewModel);
		}

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

			var loginResult = _userProvider.AuthenticateUser(model.Email, model.Password, Response, true);

			if (loginResult.Authenticated)
			{
				return RedirectToAction("Manage", new { email = loginResult.UserPrincipal.EmailAddress });
			}
				
			ModelState.AddModelError("", loginResult.ErrorText);
			return View("Index", model);
		}

		[Authorize]
		public ActionResult Logout()
		{
			_siteConfiguration.UpdateSecureMode(Request);

			// Clear the user session and forms auth ticket.
			_userProvider.Logoff(Session, Response);

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public ActionResult Manage()
		{
			_siteConfiguration.UpdateSecureMode(Request);

			var user = _userProvider.GetUserFromSelector(Request, _siteConfiguration);
			var viewModel = Mapper.Map<UserViewModel>(user);

			viewModel.SecureMode = _siteConfiguration.SecureMode;

			return View(viewModel);
		}
	}
}