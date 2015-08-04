using System.Web.Mvc;
using AutoMapper;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductProvider _productProvider;
		private readonly ISiteConfiguration _siteConfiguration;

		public ProductsController(IProductProvider productProvider, ISiteConfiguration siteConfiguration)
		{
			_productProvider = productProvider;
			_siteConfiguration = siteConfiguration;
		}

		// GET: Products
		public ActionResult Index(string subcategoryId)
		{
			_siteConfiguration.UpdateSecureMode(Request);

			var subcategory = string.IsNullOrEmpty(subcategoryId) ? _productProvider.DefaultProductCategory.ToString() : subcategoryId;
			var productsResult = _productProvider.GetProductsBySubcategoryId(subcategory);
			var viewModel = Mapper.Map<ProductsViewModel>(productsResult);

			viewModel.SecureMode = _siteConfiguration.SecureMode;

			return View(viewModel);
		}
	}
}