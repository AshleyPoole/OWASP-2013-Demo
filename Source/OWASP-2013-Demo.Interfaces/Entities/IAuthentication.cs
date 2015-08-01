namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface IAuthentication
	{
		IUser User { get; set; }
		bool Authenticated { get; set; }
		string ErrorText { get; set; }
	}
}
