using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.BusinessLogic.ProductServices
{
    public class ProductService : BaseService
    {
        public ProductService(UnitOfWork uow)
            :base(uow)
        {
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return UnitOfWork.Products.Get()
                   .Include(pd => pd.ProductDetail)
                   .ToList();
        }

        public IEnumerable<Product> GetAllProductsNotDeleted()
        {
            return UnitOfWork.Products.Get()
                   .Include(pd => pd.ProductDetail)
                   .Where(cond => cond.IsDeleted == false)
                   .ToList();
        }

        public Product GetProductById(int productId)
        {
            return UnitOfWork.Products.Get()
                .Include(pd => pd.ProductDetail)
                    .ThenInclude(prop => prop.ProductCategory)
                .FirstOrDefault(cond => cond.ProductId == productId);
        }
    }
}
