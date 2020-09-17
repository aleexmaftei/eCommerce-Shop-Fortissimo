using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.AdminVM
{
    public class AdminAddVM
    {
        [Required]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int ProductCategoryId { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public List<ProductPropertiesWithValuesVM> ProductPropertiesWithValues { get; set; }

        public AdminAddVM()
        {
            ProductPropertiesWithValues = new List<ProductPropertiesWithValuesVM>();
        }
    }
}
