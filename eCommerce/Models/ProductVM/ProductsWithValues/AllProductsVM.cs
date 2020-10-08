using eCommerce.Models.AdminVM;
using System.Collections.Generic;

namespace eCommerce.Models.ProductVM.ProductsWithValues
{
    public class AllProductsVM
    {
        public AdminUpdateVM AdminUpdateDetails { get; set; }
        public int ProductsCount { get; set; }
        public List<ProductDetailsVM> Allproducts { get; set; }
        public AllProductsVM()
        {
            Allproducts = new List<ProductDetailsVM>();
        }
    }
}
