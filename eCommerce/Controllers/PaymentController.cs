using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using eCommerce.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService PaymentService;

        private static readonly TransactionStatus[] TransactionSuccessStatuses = 
                                                    {
                                                        TransactionStatus.AUTHORIZED,
                                                        TransactionStatus.AUTHORIZING,
                                                        TransactionStatus.SETTLED,
                                                        TransactionStatus.SETTLING,
                                                        TransactionStatus.SETTLEMENT_CONFIRMED,
                                                        TransactionStatus.SETTLEMENT_PENDING,
                                                        TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                    };

        public PaymentController(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }

        [HttpGet]
        public IActionResult CheckoutPayment(double totalSum)
        {
            var clientToken = PaymentService.GenerateClientToken();
            ViewBag.ClientToken = clientToken;
            ViewBag.TotalSum = totalSum;

            return View("../Cart/Checkout");
        }
        
        [HttpPost]
        public IActionResult CheckoutPayment(string paymentMethodNonce, double totalSum)
        {
            string nonceFromTheClient = paymentMethodNonce;
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(totalSum);
            }
            catch (FormatException)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("CheckoutPayment");
            }

            var result = PaymentService.CreateTransaction(amount, nonceFromTheClient);
            
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return RedirectToAction("Show", new { id = transaction.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                return RedirectToAction("CheckoutPayment");
            }
        }

        [HttpGet]
        public IActionResult Show(string id)
        {
            var transaction = PaymentService.FindTransaction(id);
            ViewBag.Transaction = transaction;
            
            return View("../Cart/Success");
        }
    }
}
