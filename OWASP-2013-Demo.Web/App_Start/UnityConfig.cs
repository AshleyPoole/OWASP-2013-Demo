﻿using System.Web.Mvc;
using Microsoft.Practices.Unity;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces;
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
			container.RegisterInstance<ISiteConfiguration>(new SiteConfiguration(), new ContainerControlledLifetimeManager());
			return container;
		}
	}
}