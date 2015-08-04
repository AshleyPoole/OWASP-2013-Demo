using OWASP_2013_Demo.Interfaces.Entities;

namespace OWASP_2013_Demo.Interfaces.Providers
{
	public interface IRedirectProvider
	{
		IRedirect ProcessRedirection(string url);
	}
}
