namespace OWASP_2013_Demo.Interfaces.Entities
{
	public interface IAuthentication
	{
		IUserPrincipal UserPrincipal { get; set; }
		bool Authenticated { get; set; }
		string ErrorText { get; set; }
	}
}
