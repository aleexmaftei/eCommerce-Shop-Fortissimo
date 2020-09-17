using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using System.Linq;

namespace eCommerce.BusinessLogic
{
    public class UserService : BaseService
    {
        public UserService(UnitOfWork uow) 
            : base(uow)
        {
        }

        public UserT GetUserByUserId(int userId)
        {
            return UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.UserId == userId);
        }
    }
}
