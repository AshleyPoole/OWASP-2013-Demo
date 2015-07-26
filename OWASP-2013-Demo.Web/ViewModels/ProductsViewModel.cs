using System.Collections.Generic;

namespace OWASP_2013_Demo.Web.ViewModels
{
	public class ProductsViewModel : BaseViewModel
	{
		public IEnumerable<ProductViewModel> Products;
	}
}