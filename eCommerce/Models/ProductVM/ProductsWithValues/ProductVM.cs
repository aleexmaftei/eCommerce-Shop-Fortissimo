using System.Collections.Generic;

namespace eCommerce.Models.ProductVM.ProductsWithValues
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int QuantityBuy { get; set; }

        public List<PropertyWithValueVM> AllPropertiesWithValues;
        public ProductVM()
        {
            AllPropertiesWithValues = new List<PropertyWithValueVM>();
        }
    }
}
