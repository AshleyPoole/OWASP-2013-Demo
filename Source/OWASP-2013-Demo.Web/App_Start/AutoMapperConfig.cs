using System.Security.Principal;
using AutoMapper;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Models;
using OWASP_2013_Demo.Models.DB;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web
{
	public class AutoMapperConfig
	{
		public static void RegisterMapping()
		{
			Mapper.CreateMap<IProduct, ProductViewModel>();
			Mapper.CreateMap<IProductsCollection, ProductsViewModel>();
		}
	}
}