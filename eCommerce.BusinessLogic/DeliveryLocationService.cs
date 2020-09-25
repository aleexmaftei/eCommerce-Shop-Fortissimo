using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.BusinessLogic
{
    public class DeliveryLocationService : BaseService
    {
        private readonly CurrentUserDto CurrentUserDto;
       
        public DeliveryLocationService(UnitOfWork uow,
                                       CurrentUserDto currentUserCartDto)
            : base(uow)
        {
            CurrentUserDto = currentUserCartDto;
        }

        public IEnumerable<DeliveryLocation> GetDeliveryLocations()
        {
            return UnitOfWork.DeliveryLocations.Get()
                .Include(user => user.User)
                .Where(cnd => cnd.UserId == Int32.Parse(CurrentUserDto.Id))
                .ToList();
        }

        public DeliveryLocation InsertDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            return ExecuteInTransaction(uow => 
            {
                uow.DeliveryLocations.Insert(deliveryLocation);
                uow.SaveChanges();

                return deliveryLocation;
            });
        }
    }
}
