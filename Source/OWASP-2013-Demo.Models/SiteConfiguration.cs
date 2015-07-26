using System.Configuration;
using System.Web;
using OWASP_2013_Demo.Interfaces;

namespace OWASP_2013_Demo.Models
{
	public class SiteConfiguration : ISiteConfiguration
	{
		private string _websiteDomain;
		private string _dbConnection;

		public bool SecureMode { get; private set; }

		public string WebsiteDomain
		{
			get { return _websiteDomain ?? (_websiteDomain = ConfigurationManager.AppSettings["WebsiteDomain"]); }
		}

		public string DbConnection {
			get { return _dbConnection ?? (_dbConnection = ConfigurationManager.ConnectionStrings["AdventureWorksLT2012"].ConnectionString); }
		}

		public SiteConfiguration()
		{
			SecureMode = false;
		}

		public void UpdateSecureMode(HttpRequestBase request)
		{
			// if it's not spesified on the query string then don't try to update the current value
			if (request.QueryString["secure"] == null) return;

			if (request.QueryString["secure"] == "true" && this.SecureMode == false)
			{
				this.SecureMode = true;
			}
			else if (request.QueryString["secure"] == "false" && this.SecureMode == true)
			{
				this.SecureMode = false;
			}
			else
			{
				// Fall back to unsecure if unable to determin
				this.SecureMode = false;
			}
		}
	}
}