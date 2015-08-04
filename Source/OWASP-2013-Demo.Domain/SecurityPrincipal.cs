using System.Security.Principal;
using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Domain
{
	public class MySecurityPrincipal : IPrincipal
	{
		private readonly IUserPrincipal _user;

		public MySecurityPrincipal(IIdentity identity, IUserPrincipal user)
		{
			User = user;
			Identity = identity;
		}

		public IIdentity Identity { get; }

		public bool IsInRole(string role)
		{
			return true;
		}

		public IUserPrincipal User { get; }
	}
}
