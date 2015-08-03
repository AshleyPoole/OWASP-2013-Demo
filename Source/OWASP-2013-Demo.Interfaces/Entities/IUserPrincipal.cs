using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface IUserPrincipal
	{
		int CustomerID { get; set; }
		int? PersonID { get; set; }
		int BusinessEntityID { get; set; }
		string EmailAddress { get; set; }
		string Title { get; set; }
		string FirstName { get; set; }
		string MiddleName { get; set; }
		string LastName { get; set; }
	}
}
