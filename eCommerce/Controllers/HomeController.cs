using System.Linq;
using Microsoft.AspNetCore.Mvc;
using eCommerce.BusinessLogic;
using eCommerce.Models.ProductVM;

namespace eCommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductCategoryService ProductCategoryPrintService;

        public HomeController(ProductCategoryService productCategoryPrintService)
        {
            ProductCategoryPrintService = productCategoryPrintService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = ProductCategoryPrintService.GetAllBaseCategories();
            if (categories == null)
            {
                return NotFound();
            }

            var productTypeViewModel = new ProductCategoryVM()
            {
                TitleCategory = "Category",
                ProductCategories = categories.ToList()
            };

            // ++ selectie dupa brand
            return View(productTypeViewModel);
        }

        [HttpGet]
        public IActionResult Category(int id)
        {
            var categoriesById = ProductCategoryPrintService.GetAllSubcategoriesByParentId(id);
            if (categoriesById == null)
            {
                return NotFound();
            }

            var parentType = ProductCategoryPrintService.GetCategoryById(id);
            if (parentType == null)
            {
                return NotFound();
            }

            var productTypeViewModel = new ProductCategoryVM()
            {
                TitleCategory = parentType.ProductCategoryName,
                ProductCategories = categoriesById.ToList()
            };

            return View(productTypeViewModel);
        }
    }
}
