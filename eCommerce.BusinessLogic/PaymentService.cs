using Braintree;
using Braintree.Exceptions;
using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.Entities.DTOs;

namespace eCommerce.BusinessLogic
{
    public class PaymentService : BaseService
    {
        private readonly BraintreeGateway Gateway;
        private readonly CurrentUserDto CurrentUserDto;
        private readonly UserService UserService;
        private readonly DeliveryLocationService DeliveryLocationService;

        public PaymentService(UnitOfWork uow,
                              CurrentUserDto currentUserDto,
                              UserService userService,
                              DeliveryLocationService deliveryLocationService)
            :base(uow)
        {
            CurrentUserDto = currentUserDto;
            UserService = userService;
            DeliveryLocationService = deliveryLocationService;

            Gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "h4fcbb49ytxv4cdz",
                PublicKey = "cqw8bjp523gjwkzx",
                PrivateKey = "d5b06a4d3befb10fd4e0118bc4655b92"
            };
        }

        public string GenerateClientToken()
        {
            try
            {
                Customer customer = Gateway.Customer.Find(CurrentUserDto.Id);
                return Gateway.ClientToken.Generate(
                            new ClientTokenRequest
                            {
                                CustomerId = customer.Id
                            });
            } 
            catch(NotFoundException)
            {
                var currentUser = UserService.GetCurrentUser();
                if(currentUser == null)
                {
                    return null;
                }

                var customerRequest = new CustomerRequest
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Email = currentUser.Email,
                    Id = currentUser.UserId.ToString()
                };
                

                Result<Customer> resultCustomer = Gateway.Customer.Create(customerRequest);

                if (resultCustomer.IsSuccess() == true)
                {
                    return Gateway.ClientToken.Generate(
                                new ClientTokenRequest
                                {
                                    CustomerId = resultCustomer.Target.Id
                                });
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public Result<Transaction> CreateTransaction(decimal amount, string nonceFromTheClient)
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonceFromTheClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = Gateway.Transaction.Sale(transactionRequest);
            return result;
        }

        public Transaction FindTransaction(string id)
        {
            Transaction transaction = Gateway.Transaction.Find(id);
            
            return transaction;
        }
    }
}
