using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OWASP_2013_Demo.Authentication;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly ISessionData sessionData;
		private readonly IAuthenticationService authenticationService;

		public AccountController(ISessionData sessionData, IAuthenticationService authenticationService)
		{
			this.sessionData = sessionData;
			this.authenticationService = authenticationService;
		}

		private bool IsAuthenticated
		{
			get { return sessionData.Get("AuthenticatedUsername") != null; }
		}

		private string AuthenticatedUsername
		{
			get { return sessionData.Get("AuthenticatedUsername") as string; }
		}

		private void SetAuthenticated(string username)
		{
			#region Fix A2?
			//sessionData.StartNewSession();
			#endregion
			sessionData.Set("AuthenticatedUsername", username);
		}

		public ActionResult Index()
		{
			return RedirectToActionPermanent("Manage");
		}

		public ActionResult Manage()
		{
			if (!IsAuthenticated)
			{
				return RedirectToAction("Login");
			}

			ViewBag.Username = AuthenticatedUsername;

			return View(new BaseViewModel());
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View(new LoginViewModel());
		}

		[HttpPost]
		public ActionResult Login(LoginViewModel postModel)
		{
			if (ModelState.IsValid)
			{
				if (authenticationService.IsPasswordCorrectForUser(postModel.Username, postModel.Password))
				{
					SetAuthenticated(postModel.Username);

					return RedirectToAction("Manage");
				}
				else
				{
					return View(new LoginViewModel() { Username = postModel.Username, ErrorText = "Username or password is incorrect."});
				}
			}
			else
			{
				return View(new LoginViewModel() { Username = postModel.Username, ErrorText = "Some fields are invalid:" });
			}

		}

		public ActionResult Logout()
		{
			sessionData.Clear();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult _LoginPartial()
		{
			ViewBag.IsAuthenticated = IsAuthenticated;
			ViewBag.AuthenticatedUsername = AuthenticatedUsername;
			return PartialView();
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View(new RegisterViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel postData)
		{
			if (!ModelState.IsValid)
			{
				return View(new RegisterViewModel()
				{
					Username = postData.Username,
					ErrorText = "Some data is invalid."
				});
			}

			if (authenticationService.IsUsernameTaken(postData.Username))
			{
				return View(new RegisterViewModel()
				{
					Username = postData.Username,
					ErrorText = "Username is taken."
				});
			}

			if (!authenticationService.IsUsernameValid(postData.Username))
			{
				return View(new RegisterViewModel()
				{
					Username = postData.Username,
					ErrorText = "Username contains invalid characters."
				});
			}

			if (postData.Password != postData.VerifyPassword)
			{
				return View(new RegisterViewModel()
				{
					Username = postData.Username,
					ErrorText = "Passwords don't match."
				});
			}

			if (!authenticationService.IsPasswordValid(postData.Password))
			{
				return View(new RegisterViewModel()
				{
					Username = postData.Username,
					ErrorText = "Password is not complex enough."
				});
			}

			authenticationService.RegisterNewUser(postData.Username, postData.Password);
			SetAuthenticated(postData.Username);

			return RedirectToAction("Manage");
		}
	}
}