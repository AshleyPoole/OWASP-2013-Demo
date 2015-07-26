using System.Collections.Generic;
using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.DataAccess
{
	public class ProductRepository : IProductRepository
	{
		public IEnumerable<IProduct> GetProductsBy(string whereClause)
		{
			throw new System.NotImplementedException();
		}
	}
}
