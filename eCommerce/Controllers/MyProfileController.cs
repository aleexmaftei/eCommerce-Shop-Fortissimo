using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.BusinessLogic;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.DataAccess;
using eCommerce.Models.MyProfileVM;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly MyProfileService MyProfileService;
        private readonly ProductService ProductService;
        private readonly DeliveryLocationService DeliveryLocationService;
        

        public MyProfileController(MyProfileService myProfileService,
                                   ProductService productService,
                                   DeliveryLocationService deliveryLocationService)
        {
            MyProfileService = myProfileService;
            ProductService = productService;
            DeliveryLocationService = deliveryLocationService;
        }

        [HttpGet]
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

            var model = new MyProfileVm()
            {
                UserInvoices = invoicesList
            };

            return View("MyProfile", model);
        }
        [HttpGet]
        public IActionResult InsertDeliveryLocation()
        {
            var model = new MyProfileVm();
            return View("MyProfile", model);
        }

        [HttpPost]
        public IActionResult InsertDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            if(!ModelState.IsValid)
            {
                return Json(new{
                    flag = false
                });
            }

            DeliveryLocationService.InsertDeliveryLocation(deliveryLocation);
            
            return Json(new {
                flag = true
            });
        }

        [HttpGet]
        public IActionResult ModifyDeliveryLocation()
        {
            var model = new MyProfileVm();
            return View(model);
        }

        [HttpPost]
        public IActionResult ModifyDeliveryLocation(int deliveryLocationId, DeliveryLocation newDeliveryLocation)
        {

            var deliveryAdressToUpdate = DeliveryLocationService.GetDeliveryLocationById(deliveryLocationId);

            if(deliveryAdressToUpdate == null)
            {
                return Json(new{
                    flag = false
                });
            }

            deliveryAdressToUpdate.CountryName = newDeliveryLocation.CountryName;
            deliveryAdressToUpdate.RegionName = newDeliveryLocation.RegionName;
            deliveryAdressToUpdate.CityName = newDeliveryLocation.CityName;
            deliveryAdressToUpdate.AddressDetail = newDeliveryLocation.AddressDetail;
            deliveryAdressToUpdate.PostalCode = newDeliveryLocation.PostalCode;

            DeliveryLocationService.UpdateDeliveryLocation(deliveryAdressToUpdate);

            return Json(new{
                flag = true
            });
        }

        [HttpPost]
        public IActionResult DeleteDeliveryAdress(int deliveryLocationId)
        {
            var deliveryLocationToDelete = DeliveryLocationService.GetDeliveryLocationById(deliveryLocationId);
            if(deliveryLocationToDelete == null)
            {
                return Json(new {
                    flag = false
                });
            }

            if(DeliveryLocationService.DeleteDeliveryLocation(deliveryLocationToDelete) == null)
            {
                return Json(new
                {
                    flag = false
                });
            }

            return Json(new { 
                flag = true
            });
        }
    }
}
