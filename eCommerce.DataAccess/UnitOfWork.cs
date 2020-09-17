using eCommerce.DataAccess;

namespace eCommerce.Data
{
    public class UnitOfWork
    {
        private readonly EcommerceContext Context;
        public UnitOfWork(EcommerceContext context)
        {
            this.Context = context;
        }

        private BaseRepository<Product> products;
        private BaseRepository<ProductDetail> productDetails;
        private BaseRepository<Properties> properties;
        private BaseRepository<ProductCategory> productCategories;

        public BaseRepository<Product> Products => products ?? (products = new BaseRepository<Product>(Context));
        public BaseRepository<ProductDetail> ProductDetails => productDetails ?? (productDetails = new BaseRepository<ProductDetail>(Context));
        public BaseRepository<Properties> Properties => properties ?? (properties = new BaseRepository<Properties>(Context));
        public BaseRepository<ProductCategory> ProductCategories => productCategories ?? (productCategories = new BaseRepository<ProductCategory>(Context));

        
        private BaseRepository<UserT> users;
        private BaseRepository<Role> roles;
        private BaseRepository<UserRole> userRoles;
        
        public BaseRepository<UserT> Users => users ?? (users = new BaseRepository<UserT>(Context));
        public BaseRepository<Role> Roles => roles ?? (roles = new BaseRepository<Role>(Context));
        public BaseRepository<UserRole> UserRoles => userRoles ?? (userRoles = new BaseRepository<UserRole>(Context));


        private BaseRepository<Cart> carts;
        private BaseRepository<UserBuyHistory> userBuyHistories;

        public BaseRepository<Cart> Carts => carts ?? (carts = new BaseRepository<Cart>(Context));
        public BaseRepository<UserBuyHistory> UserBuyHistories => userBuyHistories ?? (userBuyHistories = new BaseRepository<UserBuyHistory>(Context));

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
