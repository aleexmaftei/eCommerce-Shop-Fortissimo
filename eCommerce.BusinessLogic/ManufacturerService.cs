using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.BusinessLogic
{
    public class ManufacturerService : BaseService
    {
        public ManufacturerService(UnitOfWork uow)
            :base(uow)
        {

        }

        public IEnumerable<Manufacturer> GetAllManufacturers()
        {
            return UnitOfWork.Manufacturers.Get()
                .ToList();
        }

        public Manufacturer GetManufacturerById(int manufacturerId)
        {
            return UnitOfWork.Manufacturers.Get()
                .FirstOrDefault(cnd => cnd.ManufacturerId == manufacturerId); 
        }
    }
}
