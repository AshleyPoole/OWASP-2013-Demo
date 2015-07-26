using System;

namespace OWASP_2013_Demo.Interfaces
{
	public interface IProduct
	{
		int ProductID { get; set; }
		string Name { get; set; }
		string ProductNumber { get; set; }
		string Color { get; set; }
		decimal StandardCost { get; set; }
		decimal ListPrice { get; set; }
		string Size { get; set; }
		decimal? Weight { get; set; }
		int? ProductCategoryID { get; set; }
		int? ProductModelID { get; set; }
		DateTime SellStartDate { get; set; }
		DateTime? SellEndDate { get; set; }
		DateTime? DiscontinuedDate { get; set; }
		byte[] ThumbNailPhoto { get; set; }
		string ThumbnailPhotoFileName { get; set; }
		Guid rowguid { get; set; }
		DateTime ModifiedDate { get; set; }
	}
}
