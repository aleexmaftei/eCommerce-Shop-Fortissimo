using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.BusinessLogic;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.Models.MyProfileVM;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly MyProfileService MyProfileService;
        private readonly ProductService ProductService;

        public MyProfileController(MyProfileService myProfileService,
                                   ProductService productService)
        {
            MyProfileService = myProfileService;
            ProductService = productService;
        }

        public IActionResult Index()
        {
            var userInvoices = MyProfileService.GetInvoices();
            if(userInvoices == null)
            {
                // nu not found, altceva
                return NotFound();
            }

            var invoicesList = new List<InvoiceVM>();
            foreach(var invoice in userInvoices)
            {
                var currentProduct = ProductService.GetProductById(invoice.ProductId);

                var invoiceModel = new InvoiceVM()
                {
                    UserBuyHistoryId = invoice.UserInvoiceId,
                    DateBuy = invoice.DateBuy,
                    ProductName = currentProduct.ProductName,
                    QuantityBuy = invoice.QuantityBuy
                };

                invoicesList.Add(invoiceModel);
            }

            var model = new InvoicesListVM()
            {
                UserInvoices = invoicesList
            };

            return View("MyProfile", model);
        }
    }
}
