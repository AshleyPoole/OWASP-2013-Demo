namespace OWASP_2013_Demo.Web.ViewModels
{
	public class ProductViewModel : BaseViewModel
	{
		public string Name { get; set; }
		public string ProductNumber { get; set; }
		public string Color { get; set; }
		public decimal ListPrice { get; set; }
		public string Size { get; set; }
		public decimal? Weight { get; set; }
		public byte[] ThumbNailPhoto { get; set; }
		public string ThumbnailPhotoFileName { get; set; }
	}
}