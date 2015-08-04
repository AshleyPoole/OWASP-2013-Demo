using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Models
{
	public class UserPrincipal : IUserPrincipal
	{
		public int CustomerID { get; set; }
		public int? PersonID { get; set; }
		public int BusinessEntityID { get; set; }
		public string EmailAddress { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string Name
		{
			get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
		}
	}
}
