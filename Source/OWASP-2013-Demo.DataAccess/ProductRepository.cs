using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Models.DB;

namespace OWASP_2013_Demo.DataAccess
{
	public class ProductRepository : IProductRepository
	{
		private readonly ISiteConfiguration _siteConfiguration;

		public ProductRepository(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}

		public IEnumerable<IProduct> GetProductsBy(string whereClause)
		{
			IEnumerable<Product> products;

			using (var sqlConnection = new SqlConnection(_siteConfiguration.DbConnection))
			{
				sqlConnection.Open();

				products = sqlConnection.Query<Product>(string.Format("SELECT * FROM SalesLT.Product {0}", whereClause)).AsEnumerable();

				sqlConnection.Close();
			}

			return products;
		}
	}
}
