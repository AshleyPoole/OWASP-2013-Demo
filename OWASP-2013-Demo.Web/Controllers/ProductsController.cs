using System.Web.Mvc;
using AutoMapper;
using OWASP_2013_Demo.Interfaces;
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
        public ActionResult Index(string categoryId)
        {
			_siteConfiguration.UpdateSecureMode(Request);

			var category = string.IsNullOrEmpty(categoryId) ? _productProvider.DefaultProductCategory.ToString() : categoryId;
	        var productsResult = _productProvider.GetProductsByCategoryId(category);
	        var viewModel = Mapper.Map<ProductsViewModel>(productsResult);

	        viewModel.SecureMode = _siteConfiguration.SecureMode;

			return View(viewModel);
        }
    }
}