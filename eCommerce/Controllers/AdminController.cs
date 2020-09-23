using AutoMapper;
using eCommerce.BusinessLogic;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs.AdminDtos;
using eCommerce.Entities.Entities.ProductAdmin;
using eCommerce.Models.AdminVM;
using eCommerce.Models.ProductVM;
using eCommerce.Models.UserAccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService AdminService;
        private readonly PropertyService PropertyService;
        private readonly ProductCategoryService ProductCategoryService;
        private readonly ProductService ProductService;
        private readonly ProductDetailsService ProductDetailsService;

        private readonly IMapper Mapper;
        public AdminController(AdminService adminService, 
                               PropertyService propertyService, 
                               ProductCategoryService productCategoryPrintService,
                               ProductService productService,
                               ProductDetailsService productDetailsService,
                               IMapper mapper)
        {
            AdminService = adminService;
            PropertyService = propertyService;
            ProductCategoryService = productCategoryPrintService;
            ProductService = productService;
            ProductDetailsService = productDetailsService;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminActionPick()
        {
            return View("AdminActionPick");
        }

        [HttpGet]
        public IActionResult AdminAddCategoryPick()
        {
            var categories = ProductCategoryService.GetAllBaseCategories();
            if (categories == null)
            {
                return NotFound();
            }

            var productTypeViewModel = new ProductCategoryVM()
            {
                TitleCategory = "Choose a category to add a product to",
                ProductCategories = categories.ToList()
            };

            return View("AdminCategoryPick", productTypeViewModel);
        }

        [HttpGet]
        public IActionResult SubcategoryPick(int parentCategoryId)
        {
            var categoriesById = ProductCategoryService.GetAllSubcategoriesByParentId(parentCategoryId);
            if (categoriesById == null)
            {
                return NotFound();
            }

            var parentType = ProductCategoryService.GetCategoryById(parentCategoryId);
            if (parentType == null)
            {
                return NotFound();
            }

            var productTypeViewModel = new ProductCategoryVM()
            {
                TitleCategory = parentType.ProductCategoryName,
                ProductCategories = categoriesById.ToList()
            };

            return View("AdminSubcategoryPick", productTypeViewModel);
        }

        [HttpGet]
        public IActionResult AddProduct(int parentCategoryId, int categoryId)
        {
            var propertiesByProduct = PropertyService.GetPropertiesByCategory(parentCategoryId);
            if (propertiesByProduct == null)
            {
                return NotFound();
            }

            var model = new AdminAddVM()
            {
                ProductCategoryId = categoryId
            };

            foreach (var property in propertiesByProduct)
            {
                var prop = new ProductPropertiesWithValuesVM()
                {
                    PropertyName = property.PropertyName,
                    PropertyId = property.PropertyId
                };

                model.ProductPropertiesWithValues.Add(prop);
            }
           

            return View("AddProduct", model);
        }

        [HttpPost]
        public IActionResult AddProduct(AdminAddVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var propertiesByInstrument = PropertyService.GetPropertiesByCategory(model.ProductCategoryId);
            if (propertiesByInstrument == null)
            {
                return NotFound();
            }

            var productMappedToDto = Mapper.Map<ProductDto>(model);
            var productMappedToEntity = Mapper.Map<AdminAdd>(productMappedToDto);

            
            AdminService.AddProductDetail(productMappedToEntity);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int productId)
        {
            var product = ProductService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var productDetails = ProductDetailsService.GetAllProductsByProductId(productId);
            if (productDetails == null)
            {
                return NotFound();
            }


            var categ = product.ProductDetail.FirstOrDefault(cnd => cnd.ProductId == productId);
            if (categ == null)
            {
                return NotFound();
            }

            var categoryId = (int)categ.ProductCategory.ParentProductCategoryId;

            var productProperties = PropertyService.GetPropertiesByCategory(categoryId);
            if (productProperties == null)
            {
                return NotFound();
            }

            var model = new AdminUpdateVM()
            {
                ProductName = product.ProductName,
                IsDeleted = product.IsDeleted,
                Quantity = product.Quantity,
                
                ProductId = productId,
                ProductCategoryId = categoryId
            };

            foreach (var property in productDetails)
            {
                var prop = new ProductPropertiesWithValuesVM()
                {
                    PropertyName = property.Property.PropertyName,
                    PropertyId = property.PropertyId,
                    DetailValue = property.DetailValue
                };

                model.ProductPropertiesWithValues.Add(prop);
            }

            return Json(new
            {
                flag = true,
                modelToDisplay = model
            }) ;
        }

        [HttpPost]
        public IActionResult UpdateProduct(AdminUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var productMappedToDto = Mapper.Map<UpdateDto>(model);
            var productMappedToEntity = Mapper.Map<AdminUpdate>(productMappedToDto);

            AdminService.UpdateProduct(productMappedToEntity);

            return Json(new {
                flag = true
            });

        }

        [HttpGet]
        public IActionResult SoftDeleteProduct(int productId)
        {
            AdminSoftDeleteVM model = new AdminSoftDeleteVM()
            {
                ProductId = productId
            };

            return View("DeleteProduct", model);
        }

        [HttpPost]
        public IActionResult SoftDeleteProduct(AdminSoftDeleteVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var productToDelete = ProductService.GetProductById(model.ProductId);
            AdminService.SoftDeleteProduct(productToDelete);

            return Json(new { 
                flag = true 
            });
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            var model = new RegisterVM();
            return View("../UserAccount/Register", model);
        }

        [HttpPost]
        public IActionResult RegisterAdmin(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = Mapper.Map<UserT>(model);

            AdminService.RegisterNewAdmin(admin);

            return RedirectToAction("Index", "Home");
        }

    }
}
