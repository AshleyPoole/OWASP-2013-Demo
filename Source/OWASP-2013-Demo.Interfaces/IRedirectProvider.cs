namespace OWASP_2013_Demo.Interfaces
{
    public interface IRedirectProvider
    {
	    IRedirect ProcessRedirection(string url);
    }
}
