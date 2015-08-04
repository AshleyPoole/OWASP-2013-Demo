using System.Collections.Generic;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Repositories
{
	public interface IProductRepository
	{
		IEnumerable<IProduct> GetProductsBy(string whereClause);
	}
}
