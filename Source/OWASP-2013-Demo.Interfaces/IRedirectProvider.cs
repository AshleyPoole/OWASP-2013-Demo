using OWASP_2013_Demo.Models;

namespace OWASP_2013_Demo.Interfaces
{
    public interface IRedirectProvider
    {
	    Redirect ProcessGoDirection(string url);
    }
}
