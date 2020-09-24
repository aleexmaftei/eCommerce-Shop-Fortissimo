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
        public UserAccountService(UnitOfWork uow)
            : base(uow)
        {
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

        public UserT RegisterNewUser(UserT user)
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

                return user;
            }); 
        }
    }
}
