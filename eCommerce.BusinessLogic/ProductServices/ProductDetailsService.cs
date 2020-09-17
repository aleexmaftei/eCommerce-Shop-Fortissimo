using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.BusinessLogic.ProductServices
{
    public class ProductDetailsService : BaseService
    {
        public ProductDetailsService(UnitOfWork uow) 
            :base(uow)
        {
        }


        public ProductDetail GetProductByProductId(int productId)
        {
            return UnitOfWork.ProductDetails.Get()
                   .Include(pt => pt.ProductCategory)
                   .Include(pr => pr.Property)
                   .FirstOrDefault(a => a.ProductId == productId);
        }

        public IEnumerable<ProductDetail> GetAllProductsNotDeletedByProductIdAndCategoryId(int productId, int categoryId)
        {
            return UnitOfWork.ProductDetails.Get()
                   .Include(pt => pt.ProductCategory)
                   .Include(pr => pr.Property)
                   .Where(a => a.ProductId == productId && a.ProductCategoryId == categoryId && a.Product.IsDeleted == false)
                   .ToList();
        }

        public IEnumerable<ProductDetail> GetAllProductsByProductId(int productId)
        {
            return UnitOfWork.ProductDetails.Get()
                   .Include(pt => pt.ProductCategory)
                   .Include(pr => pr.Property)
                   .Where(a => a.ProductId == productId)
                   .ToList();
        }
    }
}
