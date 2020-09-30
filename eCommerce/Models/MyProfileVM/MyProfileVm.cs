using System.Collections.Generic;

namespace eCommerce.Models.MyProfileVM
{
    public class MyProfileVm
    {
        public List<InvoiceVM> UserInvoices { get; set; }

        public List<DeliveryLocationVm> DeliveryLocations { get; set; }
    }
}
