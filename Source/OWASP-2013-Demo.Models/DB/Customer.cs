using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.Models.DB
{
	public class Customer : ICustomer
	{
		public int CustomerID { get; set; }
		public int? PersonID { get; set; }
		public int BusinessEntityID { get; set; }
		public string EmailAddress { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
		public int? StoreID { get; set; }
		public int? TerritoryID { get; set; }
		public string AccountNumber { get; set; }

		public string FullName
		{
			get { return this.FirstName + this.LastName; }
		}
	}
}
