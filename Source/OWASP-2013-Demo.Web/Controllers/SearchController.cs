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
		private static Queue<string> recentSearchQueries = new Queue<string>(); 
		private ISearchProvider searchProvider = new DummySearchProvider();

		
		// GET: Search
		[ValidateInput(false)]
		public ActionResult Index(string query)
		{
			recentSearchQueries.Enqueue(query);
			if (recentSearchQueries.Count > 5)
				recentSearchQueries.Dequeue();

			var results = searchProvider.Search(query);
			return View(new SearchViewModel() { Query = query, RecentQueries = recentSearchQueries, SearchResults = results });
		}
	}
}