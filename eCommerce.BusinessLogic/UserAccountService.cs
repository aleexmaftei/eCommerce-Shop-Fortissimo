using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs;
using eCommerce.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace eCommerce.BusinessLogic
{
    public class UserAccountService : BaseService
    {
        private readonly DeliveryLocationService DeliveryLocationService;
        private readonly UserService UserService;
        private readonly PasswordManagerService PasswordManagerService;
        private readonly CurrentUserDto CurrentUserDto;

        public UserAccountService(UnitOfWork uow,
                                  DeliveryLocationService deliveryLocationService,
                                  UserService userService,
                                  PasswordManagerService passwordManagerService,
                                  CurrentUserDto currentUserDto)
            : base(uow)
        {
            DeliveryLocationService = deliveryLocationService;
            UserService = userService;
            PasswordManagerService = passwordManagerService;
            CurrentUserDto = currentUserDto;
        }

        public UserT Login(string email, string password)
        {
            var userCheckLogin = UnitOfWork.Users.Get()
                        .Include(u => u.UserRole)
                            .ThenInclude(ur => ur.Role)
                        .FirstOrDefault(a => a.Email == email);

            if(userCheckLogin == null)
            {
                return null;
            }

            var saltString = UnitOfWork.Salts.Get()
                        .FirstOrDefault(salt => salt.UserId == userCheckLogin.UserId).SaltPass;
            var actualSalt = Convert.FromBase64String(saltString.Trim());

            if(PasswordManagerService.Match(password, userCheckLogin.PasswordHash, actualSalt))
            {
                return userCheckLogin;
            }

            return null;
        }

        public UserT RegisterNewUser(UserT user, DeliveryLocation deliveryLocation)
        {
            
            return ExecuteInTransaction(uow =>
            {
                string result = string.Empty;
                var salt = PasswordManagerService.HashPassword(user.PasswordHash, ref result);

                user.PasswordHash = result;

                var roleId = (int)RoleTypes.User;
                if (CurrentUserDto?.IsAdmin == true && CurrentUserDto != null)
                {
                    roleId = (int)RoleTypes.Admin;
                }

                user.UserRole = new List<UserRole>
                {
                    new UserRole
                    {
                        RoleId = roleId
                    }
                };   

                uow.Users.Insert(user);

                var userSalt = new Salt()
                {
                    User = user,
                    SaltPass = Convert.ToBase64String(salt)
                };

                uow.Salts.Insert(userSalt);

                deliveryLocation.User = user;
                uow.DeliveryLocations.Insert(deliveryLocation);

                uow.SaveChanges();
                return user;
            }); 
        }

        public UserT DeleteUser()
        {
            return ExecuteInTransaction(uow => {
                var userToDelete = UserService.GetCurrentUser();
                if(userToDelete == null)
                {
                    return userToDelete;
                }

                uow.Users.Delete(userToDelete);
                uow.SaveChanges();

                return userToDelete;
            });
        }

        public bool UpdateUserPassword(string newPassword, string oldPassword)
        {
            if(newPassword == null || oldPassword == null)
            {
                return false;
            }

            return ExecuteInTransaction(uow => {
                var userToUpdate = UserService.GetCurrentUser();
                
                if(userToUpdate == null)
                {
                    return false;
                }

                var saltString = UnitOfWork.Salts.Get()
                                .FirstOrDefault(salt => salt.UserId == userToUpdate.UserId).SaltPass;

                var oldPassActualSalt = Convert.FromBase64String(saltString.Trim());

                if (PasswordManagerService.Match(oldPassword, userToUpdate.PasswordHash, oldPassActualSalt))
                {
                    string newPasswordHash = string.Empty;
                    var newSalt = PasswordManagerService.UpdatePasswordHash(newPassword, oldPassActualSalt, ref newPasswordHash);
                   
                    
                    userToUpdate.PasswordHash = newPasswordHash;
                    uow.Users.Update(userToUpdate);
                    uow.SaveChanges();

                    return true;
                }

                return false;
            });
        }
    }
}
