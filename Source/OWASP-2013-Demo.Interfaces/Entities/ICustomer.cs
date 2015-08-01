namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface ICustomer
	{
		int CustomerID { get; set; }
		int? PersonID { get; set; }
		int BusinessEntityID { get; set; }
		string EmailAddress { get; set; }
		string Title { get; set; }
		string FirstName { get; set; }
		string MiddleName { get; set; }
		string LastName { get; set; }
		string PasswordHash { get; set; }
		string PasswordSalt { get; set; }
		int? StoreID { get; set; }
		int? TerritoryID { get; set; }
		string AccountNumber { get; set; }
		string FullName { get; }
	}
}
