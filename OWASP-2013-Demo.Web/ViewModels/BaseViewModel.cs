using System.Web;

namespace OWASP_2013_Demo.Web.ViewModels
{
	public class BaseViewModel
	{
		public bool SecureMode;

		//public BaseViewModel()
		//{
		//	SecureMode = false;
		//}

		public BaseViewModel(HttpRequestBase request)
		{
			// Default secure mode to off unless explicitly turned on
			SecureMode = false;
			SetSecureMode(request);
		}

		//public void SetSecureMode(HttpRequestBase request)
		private void SetSecureMode(HttpRequestBase request)
		{
			if (request.QueryString["secure"] == "true")
			{
				this.SecureMode = true;
			}
		}
	}
}