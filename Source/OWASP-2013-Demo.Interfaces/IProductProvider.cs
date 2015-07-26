using System;

namespace OWASP_2013_Demo.Interfaces
{
	public interface IProductProvider
	{
		IProductsCollection GetProductsByCategoryId(string id);
		String NoProductsFoundByCategoryError { get; }
		String InvalidOrMalformedCategoryError { get; }
	}
}
