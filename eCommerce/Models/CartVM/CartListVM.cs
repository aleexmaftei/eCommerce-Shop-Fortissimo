using System.Collections.Generic;

namespace eCommerce.Models.CartVM
{
    public class CartListVM
    {
        public List<CartVM> CartList { get; set; }

        public CartListVM()
        {
            CartList = new List<CartVM>();
        }
    }
}
