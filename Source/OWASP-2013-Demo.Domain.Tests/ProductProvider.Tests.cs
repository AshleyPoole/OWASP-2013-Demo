using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OWASP_2013_Demo.Domain;
using OWASP_2013_Demo.Interfaces;
using OWASP_2013_Demo.Models.DB;

namespace given_that_i_request_a_products_list
{
	[TestClass]
	public class when_I_dont_apply_best_security_practices
	{
		private static ProductProvider _productProvider;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedProductRepository = new Mock<IProductRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(false);

			// Mocked repository contains a total of 100 records. Category 18 is a valid category containing products
			// where as 99 an invalid category and doesn't contain any products.

			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 18"))
				.Returns(Enumerable.Repeat(new Product(), 2));
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99"))
				.Returns(Enumerable.Repeat(new Product(), 0));
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99 OR 1 = 1"))
				.Returns(Enumerable.Repeat(new Product(), 100));
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99; SELECT * FROM SalesLT.Customer"))
				.Returns(Enumerable.Repeat(new Product(), 100));

			_productProvider = new ProductProvider(mockedProductRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void using_a_valid_id_i_should_receive_a_products_list()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18");
			result.Products.Count().Should().Be(2);
		}

		[TestMethod]
		public void using_a_valid_id_i_should_not_receive_an_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18");
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void using_a_invalid_id_i_should_receive_an_empty_products_list()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99");
			result.Products.Count().Should().Be(0);
		}

		[TestMethod]
		public void using_a_invalid_id_i_should_receive_an_empty_products_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99");
			result.ErrorText.Should().Be(_productProvider.NoProductsFoundByCategoryError);
		}

		[TestMethod]
		public void using_a_valid_id_plus_extra_injected_parameters_i_should_receive_a_full_products_list()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99 OR 1 = 1");
			result.Products.Count().Should().Be(100);
		}

		[TestMethod]
		public void using_a_valid_id_plus_extra_injected_parameters_i_should_not_receive_an_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99 OR 1 = 1");
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void using_a_valid_id_plus_extra_injected_parameters_i_should_receive_a_full_products_list_and_no_error_if_selecting_all_customers_too()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99; SELECT * FROM SalesLT.Customer");
			result.Products.Count().Should().Be(100);
		}
	}

	[TestClass]
	public class when_I_apply_best_security_practices
	{
		private static ProductProvider _productProvider;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var mockedProductRepository = new Mock<IProductRepository>();
			var mockedSiteConfiguration = new Mock<ISiteConfiguration>();

			mockedSiteConfiguration.Setup(x => x.SecureMode).Returns(true);

			// Mocked repository contains a total of 100 records. Category 18 is a valid category containing products
			// where as 99 an invalid category and doesn't contain any products.

			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 18"))
				.Returns(Enumerable.Repeat(new Product(), 2));
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99"))
				.Returns(Enumerable.Repeat(new Product(), 0));
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99 OR 1 = 1"))
				.Returns((IEnumerable<IProduct>) null);
			mockedProductRepository.Setup(x => x.GetProductsBy("WHERE ProductCategoryID = 99; SELECT * FROM SalesLT.Customer"))
				.Returns((IEnumerable<IProduct>) null);

			_productProvider = new ProductProvider(mockedProductRepository.Object, mockedSiteConfiguration.Object);
		}

		[TestMethod]
		public void using_a_valid_id_i_should_receive_a_products_list()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18");
			result.Products.Count().Should().Be(2);
		}

		[TestMethod]
		public void using_a_valid_id_i_should_not_receive_an_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18");
			result.ErrorText.Should().BeNullOrEmpty();
		}

		[TestMethod]
		public void using_a_invalid_id_i_should_receive_an_empty_products_list()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99");
			result.Products.Count().Should().Be(0);
		}

		[TestMethod]
		public void using_a_invalid_id_i_should_receive_an_empty_products_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("99");
			result.ErrorText.Should().Be(_productProvider.NoProductsFoundByCategoryError);
		}

		[TestMethod]
		public void using_a_valid_id_plus_extra_injected_parameters_should_throw_an_error()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18; SELECT * FROM SalesLT.Customer");
			result.ErrorText.Should().Be(_productProvider.InvalidOrMalformedCategoryError);
		}

		[TestMethod]
		public void using_a_valid_id_plus_extra_injected_parameters_should_return_null_products()
		{
			var result = _productProvider.GetProductsBySubcategoryId("18; SELECT * FROM SalesLT.Customer");
			result.Products.Should().BeNullOrEmpty();
		}
	}
}
