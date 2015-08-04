using System.Web;

namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface ISiteConfiguration
	{
		bool SecureMode { get; }
		string WebsiteDomain { get; }
		void UpdateSecureMode(HttpRequestBase request);
		string DbConnection { get; }
	}
}
