using AutoMapper;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.DataAccess;
using eCommerce.Models.ProductVM;
using eCommerce.Models.ProductVM.ProductsWithValues;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace eCommerce.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly ProductDetailsService ProductDetailsService;
        private readonly ProductService ProductService;
        private readonly ProductCommentService ProductCommentService;
        private readonly IMapper Mapper;

        public ProductDetailsController(ProductDetailsService productDetailsService,
                                        ProductService productService,
                                        ProductCommentService productCommentService,
                                        IMapper mapper)
        {
            ProductDetailsService = productDetailsService;
            ProductService = productService;
            ProductCommentService = productCommentService;
            Mapper = mapper;
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
                var propertiesList = new ProductDetailsVM()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductImage = product.ProductImage,
                    ManufacturerLogo = product.Manufacturer.ManufacturerLogo,
                    ManufacturerName = product.Manufacturer.ManufacturerName
                };

                var productsByProductId = ProductDetailsService.GetAllProductDetailsNotDeleted(product.ProductId, categoryId);
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

        [HttpGet]
        public IActionResult ViewFullSpecsProduct(int productId)
        {
            var currentProduct = ProductService.GetProductById(productId);
            if(currentProduct == null)
            {
                return NotFound();
            }

            var currentProductDetails = ProductDetailsService.GetAllProductDetailsByProductId(productId);
            if(currentProductDetails == null)
            {
                return NotFound();
            }

            var productModel = new ProductDetailsVM()
            {
                ManufacturerLogo = currentProduct.Manufacturer.ManufacturerLogo,
                ManufacturerName = currentProduct.Manufacturer.ManufacturerName,
                ProductId = currentProduct.ProductId,
                ProductName = currentProduct.ProductName,
                ProductPrice = currentProduct.ProductPrice,
                ProductImage = currentProduct.ProductImage,
                QuantityBuy = 1
            };

            foreach (var productByProductId in currentProductDetails)
            {
                productModel.AllPropertiesWithValues.Add(new PropertyWithValueVM()
                {
                    PropertyName = productByProductId.Property.PropertyName,
                    Value = productByProductId.DetailValue
                });
            }


            var modelProductComments = new List<ProductCommentVM>();
            var currentProductComments = ProductCommentService.GetAllProductCommentsByProductId(productId).ToList();

            foreach (var productComment in currentProductComments)
            {
                var modelMapped = Mapper.Map<ProductCommentVM>(productComment);
                modelProductComments.Add(modelMapped);
            }

            var numberOfEvaluations = currentProductComments.Count();
            var model = new ProductInfoVM()
            {
                IsDeleted = currentProduct.IsDeleted,
                Quantity = currentProduct.Quantity,
                NumberOfEvaluations = numberOfEvaluations,
                ProductDetails = productModel,
                ProductComments = modelProductComments
            };

            return View("../Products/ProductDetails", model);
        }
    }

}
