using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.BusinessLogic
{
    public class UserAccountService : BaseService
    {
        public readonly DeliveryLocationService DeliveryLocationService;
        public UserAccountService(UnitOfWork uow,
                                  DeliveryLocationService deliveryLocationService)
            : base(uow)
        {
            DeliveryLocationService = deliveryLocationService;
        }

        public UserT Login(string email, string password)
        {
            // TODO: hash the password
            var passwordHash = password;
            
            return UnitOfWork.Users.Get()
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefault(a => a.Email == email && a.PasswordHash == passwordHash);
        }

        public UserT RegisterNewUser(UserT user, DeliveryLocation deliveryLocation)
        {
            return ExecuteInTransaction(uow =>
            {
                user.UserRole = new List<UserRole>
                {
                    new UserRole
                    {
                        RoleId = (int)RoleTypes.User
                    }
                };

                uow.Users.Insert(user);
                uow.SaveChanges();

                deliveryLocation.UserId = user.UserId;
                DeliveryLocationService.InsertDeliveryLocation(deliveryLocation);
               
                return user;
            }); 
        }
    }
}
