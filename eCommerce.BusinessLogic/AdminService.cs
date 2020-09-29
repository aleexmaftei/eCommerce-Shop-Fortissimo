using eCommerce.BusinessLogic.Base;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.Entities.ProductAdmin;
using eCommerce.Entities.Enums;
using System.Collections.Generic;

namespace eCommerce.BusinessLogic
{
    public class AdminService : BaseService
    {
        private readonly ProductService ProductService;
        
        public AdminService(UnitOfWork uow, ProductService productService)
            :base(uow)
        {
            ProductService = productService;
        }

        public UserT RegisterNewAdmin(UserT admin)
        {
            return ExecuteInTransaction(uow =>
            {
                admin.UserRole = new List<UserRole>
                {
                    new UserRole
                    {
                        RoleId = (int)RoleTypes.Admin
                    }
                };

                uow.Users.Insert(admin);

                uow.SaveChanges();

                return admin;
            });
        }

        public AdminAdd AddProductDetail(AdminAdd productMappedToEntity)
        {
            return ExecuteInTransaction(uow => 
            {
                var product = new Product
                {
                    ProductName = productMappedToEntity.ProductName,
                    Quantity = productMappedToEntity.Quantity
                };

                uow.Products.Insert(product);
                uow.SaveChanges();

                var products = new List<ProductDetail>();
                foreach (var value in productMappedToEntity.ProductPropertiesWithValues)
                {
                    var productDetail = new ProductDetail();
                    productDetail.ProductId = product.ProductId;
                    productDetail.ProductCategoryId = productMappedToEntity.ProductCategoryId;
                    
                    productDetail.DetailValue = value.DetailValue;
                    productDetail.PropertyId = value.PropertyId;
                    
                    products.Add(productDetail);
                }

                uow.ProductDetails.InsertList(products);
                uow.SaveChanges();

                return productMappedToEntity;
            });
        }

        public void SoftDeleteProduct(Product product)
        {
            ExecuteInTransaction(uow =>
            {
                product.Quantity = 0;
                product.IsDeleted = true;
                uow.Products.Update(product);
                //uow.SaveChanges();

                return product;
            }); 
        }

        public AdminUpdate UpdateProduct(AdminUpdate product)
        {
            return ExecuteInTransaction(uow => 
            {
                var productToUpdate = ProductService.GetProductById(product.ProductId);

                productToUpdate.ProductName = product.ProductName;
                productToUpdate.IsDeleted = product.IsDeleted;
                productToUpdate.Quantity = product.Quantity;

                var index = 0;
                foreach(var productDetail in productToUpdate.ProductDetail)
                {
                    productDetail.DetailValue = product.ProductPropertiesWithValues[index].DetailValue;
                    index++;
                }

                uow.Products.Update(productToUpdate);

                uow.SaveChanges();

                return product;
            });
        }
    }
}
