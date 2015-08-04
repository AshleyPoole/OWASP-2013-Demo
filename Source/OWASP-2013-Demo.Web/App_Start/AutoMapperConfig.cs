using AutoMapper;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Web.ViewModels;

namespace OWASP_2013_Demo.Web
{
	public class AutoMapperConfig
	{
		public static void RegisterMapping()
		{
			Mapper.CreateMap<IProduct, ProductViewModel>();
			Mapper.CreateMap<IProductsCollection, ProductsViewModel>();
			Mapper.CreateMap<IUser, CustomerViewModel>();
			Mapper.CreateMap<IUser, IUserPrincipal>();
		}
	}
}