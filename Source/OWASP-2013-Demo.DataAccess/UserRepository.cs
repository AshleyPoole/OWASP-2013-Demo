using System.Data.SqlClient;
using System.Linq;
using Dapper;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Models.DB;

namespace OWASP_2013_Demo.DataAccess
{
	public class UserRepository : IUserRepository
	{
		private readonly ISiteConfiguration _siteConfiguration;

		public UserRepository(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}
		public IUser FetchUserByEmailAddress(string emailAddress)
		{
			User user;

			using (var sqlConnection = new SqlConnection(_siteConfiguration.DbConnection))
			{
				sqlConnection.Open();

				user = sqlConnection.Query<User>(@"SELECT CustomerID, PersonID, per.BusinessEntityID, EmailAddress, Title, 
												FirstName, MiddleName, LastName, PasswordHash, PasswordSalt, StoreID, 
												TerritoryID, AccountNumber
												FROM Person.EmailAddress emad
												INNER JOIN Person.Person per ON emad.BusinessEntityID = per.BusinessEntityID
												INNER JOIN Sales.Customer cust ON cust.PersonID = per.BusinessEntityID
												INNER JOIN Person.Password pass ON per.BusinessEntityID = pass.BusinessEntityID
												WHERE EmailAddress = @Email", new {Email = emailAddress}).FirstOrDefault();

				sqlConnection.Close();
			}

			return user;
		}
	}
}
