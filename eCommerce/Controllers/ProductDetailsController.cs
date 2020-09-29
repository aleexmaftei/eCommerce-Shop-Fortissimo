using eCommerce.BusinessLogic.ProductServices;
using eCommerce.Models.ProductVM.ProductsWithValues;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eCommerce.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly ProductDetailsService ProductDetailsService;
        private readonly ProductService ProductService;

        public ProductDetailsController(ProductDetailsService productDetailsService,
                                        ProductService productService)
        {
            ProductDetailsService = productDetailsService;
            ProductService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListProductDetailsByCategory(int categoryId)
        {
            var products = ProductService.GetAllProductsNotDeleted();
            if(products == null)
            {
                return NotFound();
            }

            var allProducts = new AllProductsVM()
            {
                ProductsCount = products.Count()
            };

            foreach (var product in products)
            {
                var propertiesList = new ProductVM()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice
                };

                var productsByProductId = ProductDetailsService.GetAllProductsNotDeletedByProductIdAndCategoryId(product.ProductId, categoryId);
                foreach (var productByProductId in productsByProductId)
                {
                    propertiesList.AllPropertiesWithValues.Add(new PropertyWithValueVM()
                    {
                        PropertyName = productByProductId.Property.PropertyName,
                        Value = productByProductId.DetailValue
                    });

                }

                allProducts.Allproducts.Add(propertiesList);
            }

            return View("../Products/Products", allProducts);
        }
    }
}
