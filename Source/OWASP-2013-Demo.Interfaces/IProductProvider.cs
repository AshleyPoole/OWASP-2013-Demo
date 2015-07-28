using System;

namespace OWASP_2013_Demo.Interfaces
{
	public interface IProductProvider
	{
		IProductsCollection GetProductsBySubcategoryId(string id);
		String NoProductsFoundByCategoryError { get; }
		String InvalidOrMalformedCategoryError { get; }
		int DefaultProductCategory { get; }
	}
}
