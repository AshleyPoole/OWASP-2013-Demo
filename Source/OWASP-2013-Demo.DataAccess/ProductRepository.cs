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

			const string query = @"SELECT prod.ProductID, prod.Name ,prod.ProductNumber, prod.Color,
					 prod.StandardCost, prod.ListPrice, prod.Size, prod.[Weight],
					 prod.ProductSubcategoryID, prod.ProductModelID, prod.SellStartDate, prod.SellEndDate,
					 prod.DiscontinuedDate, pp.ThumbNailPhoto, pp.ThumbnailPhotoFileName, prod.rowguid, prod.ModifiedDate
					 FROM Production.Product prod
					 INNER JOIN Production.ProductProductPhoto ppp ON ppp.ProductID = prod.ProductID
					 INNER JOIN Production.ProductPhoto pp ON pp.ProductPhotoID = ppp.ProductPhotoID";

			using (var sqlConnection = new SqlConnection(_siteConfiguration.DbConnection))
			{
				sqlConnection.Open();

				products = sqlConnection.Query<Product>(string.Format("{0} {1}", query, whereClause)).AsEnumerable();

				sqlConnection.Close();
			}

			return products;
		}
	}
}
