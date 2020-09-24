using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.BusinessLogic
{
    public class MyProfileService : BaseService
    {
        private readonly CurrentUserDto CurrentUserDto;

        public MyProfileService(UnitOfWork uow, 
                                CurrentUserDto currentUserDto)
            : base(uow)
        {
            CurrentUserDto = currentUserDto;
        }

        public IEnumerable<UserBuyHistory> GetInvoices()
        {
            var currentUserId = Int32.Parse(CurrentUserDto.Id);

            return UnitOfWork.UserBuyHistories.Get()
                .Include(prd => prd.Product)
                .Where(cnd => cnd.UserId == currentUserId)
                .ToList();
        }
    }
}
