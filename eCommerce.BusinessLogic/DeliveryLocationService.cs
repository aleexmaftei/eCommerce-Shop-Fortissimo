using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public DeliveryLocation GetDeliveryLocationById(int deliveryLocation)
        {
            return UnitOfWork.DeliveryLocations.Get()
                .Include(user => user.User)
                .FirstOrDefault(cnd => cnd.DeliveryLocationId == deliveryLocation && cnd.UserId == Int32.Parse(CurrentUserDto.Id));
        }

        public DeliveryLocation InsertDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            if(deliveryLocation == null)
            {
                return deliveryLocation;
            }

            return ExecuteInTransaction(uow => 
            {
                uow.DeliveryLocations.Insert(deliveryLocation);
                uow.SaveChanges();

                return deliveryLocation;
            });
        }

        public DeliveryLocation UpdateDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            if(deliveryLocation == null)
            {
                return deliveryLocation;
            }

            return ExecuteInTransaction(uow => {

                uow.DeliveryLocations.Update(deliveryLocation);
                uow.SaveChanges();

                return deliveryLocation;
            });
        }

        public DeliveryLocation DeleteDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            if(deliveryLocation == null)
            {
                return deliveryLocation;
            }

            return ExecuteInTransaction(uow => {
                
                uow.DeliveryLocations.Delete(deliveryLocation);
                uow.SaveChanges();
                
                return deliveryLocation;
            });
            
        }
    }
}
