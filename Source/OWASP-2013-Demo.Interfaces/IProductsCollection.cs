﻿using System.Collections.Generic;

namespace OWASP_2013_Demo.Interfaces
{
	public interface IProductsCollection
	{
		IEnumerable<IProduct> Products { get; set; }
		string ErrorText { get; set; }
	}
}
