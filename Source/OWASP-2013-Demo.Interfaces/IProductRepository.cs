using System.Collections.Generic;

namespace OWASP_2013_Demo.Interfaces
{
	public interface IProductRepository
	{
		IEnumerable<IProduct> GetProductsBy(string whereClause);
	}
}
