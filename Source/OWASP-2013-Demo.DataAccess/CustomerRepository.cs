using System.Data.SqlClient;
using Dapper;
using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.DataAccess
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly ISiteConfiguration _siteConfiguration;

		public CustomerRepository(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}
		public dynamic FetchUserByEmailAddress(string emailAddress)
		{
			dynamic result;

			using (var sqlConnection = new SqlConnection(_siteConfiguration.DbConnection))
			{
				sqlConnection.Open();

				result = sqlConnection.Query(@"SELECT CustomerID, PersonID, per.BusinessEntityID, EmailAddress, Title, 
												FirstName, MiddleName, LastName, PasswordHash, PasswordSalt, StoreID, 
												TerritoryID, AccountNumber
												FROM Person.EmailAddress emad
												INNER JOIN Person.Person per ON emad.BusinessEntityID = per.BusinessEntityID
												INNER JOIN Sales.Customer cust ON cust.PersonID = per.BusinessEntityID
												INNER JOIN Person.Password pass ON per.BusinessEntityID = pass.BusinessEntityID
												WHERE EmailAddress = @Email", new {Email = emailAddress});

				sqlConnection.Close();
			}

			return result;
		}
	}
}
