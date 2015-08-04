using System.Collections.Generic;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Models
{
	public class ProductsCollection : IProductsCollection
	{
		public IEnumerable<IProduct> Products { get; set; }
		public string ErrorText { get; set; }
	}
}
