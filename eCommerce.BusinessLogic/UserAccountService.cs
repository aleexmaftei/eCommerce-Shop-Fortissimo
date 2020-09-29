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
        public readonly UserService UserService;

        public UserAccountService(UnitOfWork uow,
                                  DeliveryLocationService deliveryLocationService,
                                  UserService userService)
            : base(uow)
        {
            DeliveryLocationService = deliveryLocationService;
            UserService = userService;
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

        public UserT DeleteUser()
        {
            return ExecuteInTransaction(uow => {
                var userToDelete = UserService.GetCurrentUser();
                uow.Users.Delete(userToDelete);
                uow.SaveChanges();

                return userToDelete;
            });
        }

        public UserT UpdateUserPassword(UserT user)
        {
            if(user == null)
            {
                return user;
            }

            return ExecuteInTransaction(uow => {
                
                uow.Users.Update(user);
                uow.SaveChanges();

                return user;
            });
        }
    }
}
