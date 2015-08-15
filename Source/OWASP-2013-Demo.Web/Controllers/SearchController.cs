using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OWASP_2013_Demo.Search;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class SearchController : Controller
	{
		private ISearchProvider searchProvider = new DummySearchProvider();

		// GET: Search
		public ActionResult Index(string query)
		{
			var results = searchProvider.Search(query);
			return View(new SearchViewModel() { Query = query, SearchResults = results });
		}
	}
}