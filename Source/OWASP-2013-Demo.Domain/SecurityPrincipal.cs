using System.Security.Principal;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Domain
{
	public class MySecurityPrincipal : IPrincipal
	{
		public MySecurityPrincipal(IIdentity identity)
		{
			Identity = identity;
		}

		public IIdentity Identity { get; }

		public bool IsInRole(string role)
		{
			return true;
		}

		public IUserPrincipal User { get; set; }
	}
}
