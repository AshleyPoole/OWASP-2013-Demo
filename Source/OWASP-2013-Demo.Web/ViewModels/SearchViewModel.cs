using System.Collections.Generic;
using OWASP_2013_Demo.Search;

namespace OWASP_2013_Demo.Web.ViewModels
{
	public class SearchViewModel : BaseViewModel
	{
		public string Query { get; set; }

		public IEnumerable<SearchResult> SearchResults { get; set; }

		public IEnumerable<string> RecentQueries { get; set; }
	}
}