namespace OWASP_2013_Demo.Web.ViewModels
{
	public class BaseViewModel
	{
		public bool SecureMode;
		public string ErrorText;

		public BaseViewModel()
		{
			SecureMode = false;
		}
	}
}