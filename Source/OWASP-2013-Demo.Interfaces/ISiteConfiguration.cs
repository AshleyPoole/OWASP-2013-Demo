using System.Web;

namespace OWASP_2013_Demo.Interfaces
{
	public interface ISiteConfiguration
	{
		bool SecureMode { get; }
		string WebsiteDomain { get; }
		void UpdateSecureMode(HttpRequestBase request);
	}
}
