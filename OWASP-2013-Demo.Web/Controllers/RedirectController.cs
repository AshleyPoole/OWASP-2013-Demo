using System.Web.Mvc;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Web.Models;

namespace OWASP_2013_Demo.Web.Controllers
{
    public class RedirectController : Controller
    {
	    private readonly IRedirectProvider _redirectProvider;

	    public RedirectController(IRedirectProvider redirectProvider)
	    {
		    _redirectProvider = redirectProvider;
	    }

	    // GET: Go
        public ActionResult Index(RedirectRequest redirectRequest)
        {
			var redirectResponse = _redirectProvider.ProcessGoDirection(redirectRequest.Url);

			// Redirect client to requested url if allowed
	        if (redirectResponse.Allowed) return Redirect(redirectResponse.UrlForRedirect.ToString());

			// Else return the error
	        redirectRequest.Error = redirectResponse.ErrorMessage;
	        return Index(redirectRequest);
        }
    }
}