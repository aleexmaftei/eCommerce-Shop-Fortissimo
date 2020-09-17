using eCommerce.BusinessLogic.Base;
using eCommerce.Data;
using eCommerce.DataAccess;
using eCommerce.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.BusinessLogic.ProductServices
{
    public class CartService : BaseService
    {
        private readonly CurrentUserDto CurrentUserCartDto;
        private readonly UserService UserService;
        private readonly ProductService ProductService;


        public CartService(UnitOfWork uow, 
                           CurrentUserDto currentUserCartDto, 
                           UserService userService,
                           ProductService productService)
            :base(uow)
        {
            CurrentUserCartDto = currentUserCartDto;
            UserService = userService;
            ProductService = productService;
        }

        public IEnumerable<Cart> GetAllCartProductsNotDeletedNotOrderPlaced()
        {
            return UnitOfWork.Carts.Get()
                .Include(c => c.Product)
                .Where(cnd => cnd.UserId == Int32.Parse(CurrentUserCartDto.Id) && 
                              cnd.Product.IsDeleted == false)
                .ToList();
        }

        public Cart GetCartByProductIdAndUser(int productId)
        {
            return UnitOfWork.Carts.Get()
                .FirstOrDefault(cnd => cnd.UserId == Int32.Parse(CurrentUserCartDto.Id) && cnd.ProductId == productId);
        }

        public Cart InsertToCart(Cart cart)
        {
            var currentUserId = Int32.Parse(CurrentUserCartDto.Id);

            var currentUser = UserService.GetUserByUserId(currentUserId);
            if (currentUser == null)
            {
                return cart;
            }
            
            return ExecuteInTransaction(uow =>
            {
                cart.UserId = currentUser.UserId;

                uow.Carts.Insert(cart);
                uow.SaveChanges();

                return cart;
            });
        }

        public IEnumerable<Cart> PlaceOrder(List<Cart> carts)
        {
            var currentUserId = Int32.Parse(CurrentUserCartDto.Id);

            var currentUser = UserService.GetUserByUserId(currentUserId);
            if (currentUser == null)
            {
                return carts;
            }

            return ExecuteInTransaction(uow =>
            {
                var buyHistoryList = new List<UserBuyHistory>();

                foreach (var cart in carts)
                {
                    var currentProduct = ProductService.GetProductById(cart.ProductId);
                    if(currentProduct == null)
                    {
                        return carts;
                    }

                    var userBuyHistory = new UserBuyHistory()
                    {
                        UserId = currentUser.UserId,
                        QuantityBuy = cart.QuantityBuy,
                        ProductId = currentProduct.ProductId
                    };

                    buyHistoryList.Add(userBuyHistory);
                }

                DeleteAllProductsFromCart(carts);

                uow.UserBuyHistories.InsertList(buyHistoryList);
                uow.SaveChanges();
                
                return carts;
            });
        }
        
        private void DeleteAllProductsFromCart(List<Cart> carts)
        {
            foreach(var cart in carts)
            {
                UnitOfWork.Carts.Delete(cart);
            }
        }

        public void DeleteProductFromCart(Cart cart)
        {
            UnitOfWork.Carts.Delete(cart);
            UnitOfWork.SaveChanges();
        }

    }
}
