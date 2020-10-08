using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.Entities.ProductAdmin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.BusinessLogic.ProductServices
{
    public class ProductService : BaseService
    {
        public ProductService(UnitOfWork uow)
            : base(uow)
        {
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return UnitOfWork.Products.Get()
                   .Include(pd => pd.ProductDetail)
                   .Include(man => man.Manufacturer)
                   .ToList();
        }

        public IEnumerable<Product> GetAllProductsNotDeleted()
        {
            return UnitOfWork.Products.Get()
                   .Include(pd => pd.ProductDetail)
                   .Include(man => man.Manufacturer)
                   .Where(cond => cond.IsDeleted == false)
                   .ToList();
        }

        public List<MostWantedProducts> GetMostWantedProducts(int numberOfProductsToView)
        {
            return UnitOfWork.UserInvoices.Get()
                .Include(ui => ui.Product)
                    .ThenInclude(man => man.Manufacturer)
                .GroupBy(x =>  new { x.ProductId, x.Product.ProductName, x.Product.ProductImage })
                .OrderByDescending(s => s.Count())
                .Select(s => new MostWantedProducts { ProductId = s.Key.ProductId, 
                                                      ProductName = s.Key.ProductName,
                                                      ProductImage = s.Key.ProductImage,
                                                      ProductsSold = s.Count()})
                .Take(numberOfProductsToView)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return UnitOfWork.Products.Get()
                .Include(man => man.Manufacturer)
                .Include(pc => pc.ProductComment)
                .Include(pd => pd.ProductDetail)
                    .ThenInclude(prodcat => prodcat.ProductCategory)
                .FirstOrDefault(cond => cond.ProductId == productId);
        }
    }
}
