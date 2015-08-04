using System;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IProductProvider
	{
		IProductsCollection GetProductsBySubcategoryId(string id);
		String NoProductsFoundByCategoryError { get; }
		String InvalidOrMalformedCategoryError { get; }
		int DefaultProductCategory { get; }
	}
}
