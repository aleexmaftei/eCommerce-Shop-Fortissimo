using System;

namespace eCommerce.Models.MyProfileVM
{
    public class InvoiceVM
    {
        public int UserBuyHistoryId { get; set; }
        public DateTime DateBuy { get; set; }
        public string ProductName { get; set; }
        public int QuantityBuy { get; set; }
    }
}
