using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using OWASP_2013_Demo.Authentication;
using OWASP_2013_Demo.Authentication.Encryption;
using OWASP_2013_Demo.DataAccess;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Interfaces.Utilities;
using OWASP_2013_Demo.Models;
using Unity.Mvc5;

namespace OWASP_2013_Demo.Web
{
	public static class UnityConfig
	{
		public static void RegisterComponents()
		{
			var container = GetUnityContainer();
			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}

		public static IUnityContainer GetUnityContainer()
		{
			var container = new UnityContainer();

			container.RegisterType<IRedirectProvider, RedirectProvider>();
			container.RegisterType<IProductProvider, ProductProvider>();
			container.RegisterType<IProductRepository, ProductRepository>();

			container.RegisterType<ISessionData, InMemorySessionData>();
			container.RegisterType<HttpContextBase>(new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));

			var testAuthenticationDatabase = new InMemoryAuthenticationDatabase();
            container.RegisterInstance<IAuthenticationDatabase>(testAuthenticationDatabase);
			container.RegisterType<IAuthenticationService, ExampleAuthenticationService>();
			container.RegisterType<IPasswordValidator, SimpleHashPasswordValidator>();

			container.RegisterInstance<ISiteConfiguration>(new SiteConfiguration(), new ContainerControlledLifetimeManager());
			
			return container;
		}
	}
}