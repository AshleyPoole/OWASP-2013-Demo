# OWASP-2013-Demo
# Intro #
This website demonstrates [OWASP's top 10](https://www.owasp.org/index.php/Top_10_2013-Top_10) vulnerabilities which are:  
- A1 Injection  
- A2 Broken authentication and session management  
- A3 Cross-site scripting (XSS)  
- A4 Insecure direct object references  
- A5 Security misconfiguration  
- A6 Sensitive data exposure  
- A7 Missing function control  
- A8 Cross site request forgery (CSRF)  
- A9 Using components with known vulnerabilities  
- A10 Unvalidated redirects and forwards  
  
  
An "about" will be added shortly to the website explaining how these can be exploited and defended against.  
  
  
# Set up #
If you would like to run this website up for yourself, then you will require the following items. Alternatively, the master branch is build and deployed to [https://supersecure.website](https://supersecure.website).  
1. The AdventureWorks2012 database installed either on [SQL Server](http://msftdbprodsamples.codeplex.com/releases/view/55330) or [Azure SQL](http://msftdbprodsamples.codeplex.com/releases/view/37304).  
2. IIS7 or Express installed with .NET 4.5 or greater.
3. Modify web.config within OWASP-2013-Demo.Web project and update the DB connection string called AdventureWorks2012 to your settings. Also update the app settings called WebsiteDomain to the domain name running your website (i.e localhost if running locally or supersecure.website).  
4. Now you should be ready to run the website within Visual Studio or build it and deploy to another location.  