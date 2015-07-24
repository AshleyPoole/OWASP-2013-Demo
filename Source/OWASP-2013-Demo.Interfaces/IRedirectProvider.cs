namespace OWASP_2013_Demo.Interfaces
{
    public interface IRedirectProvider
    {
	    IRedirectObject ProcessGoDirection(string url);
    }
}
