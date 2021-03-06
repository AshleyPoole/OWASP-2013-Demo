﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using OWASP_2013_Demo.Interfaces.Entities;
using OWASP_2013_Demo.Interfaces.Providers;
using OWASP_2013_Demo.Interfaces.Repositories;
using OWASP_2013_Demo.Models;

namespace OWASP_2013_Demo.Domain
{
	public class ProductProvider : IProductProvider
	{
		private readonly IProductRepository _productRepository;
		private readonly ISiteConfiguration _siteConfiguration;
		public string NoProductsFoundByCategoryError { get; private set; }
		public string InvalidOrMalformedCategoryError { get; private set; }
		public int DefaultProductCategory { get; private set; }

		public ProductProvider(IProductRepository productRepository, ISiteConfiguration siteConfiguration)
		{
			_productRepository = productRepository;
			_siteConfiguration = siteConfiguration;

			NoProductsFoundByCategoryError = "No products were found for the selected category. Please make another selection.";
			InvalidOrMalformedCategoryError = "An invalid or malformed category was selected. Please make another selection.";
			DefaultProductCategory = 18;
		}

		public IProductsCollection GetProductsBySubcategoryId(string id)
		{
			var queryParameter = id;
			var productsCollection = new ProductsCollection();

			if (_siteConfiguration.SecureMode && !QueryParameterValid(queryParameter))
			{
				// Secure mode is active and query parameter didn't meet validation
				productsCollection.ErrorText = InvalidOrMalformedCategoryError;
				return productsCollection;
			}

			productsCollection.Products = _productRepository.GetProductsBy(string.Format("WHERE ProductSubcategoryID = {0}", queryParameter));

			if (productsCollection.Products == null || !productsCollection.Products.Any())
			{
				productsCollection.ErrorText = NoProductsFoundByCategoryError;
			}

			return productsCollection;
		}

		private static bool QueryParameterValid(string uncleansedParameter)
		{
			int intOut;
			var regex = new Regex(@"^0*[1-9][0-9]*$");

			return regex.IsMatch(uncleansedParameter) && Int32.TryParse(uncleansedParameter, out intOut);
		}
	}
}
