﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OWASP_2013_Demo.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			UnityConfig.RegisterComponents();
			AutoMapperConfig.RegisterMapping();
		}
	}
}
