using AutoMapper;
using eCommerce.BusinessLogic.ProductServices;
using eCommerce.DataAccess;
using eCommerce.Models.CartVM;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eCommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService CartService;
        private readonly ProductService ProductService;

        private readonly IMapper Mapper;
        public CartController(CartService cartService, 
                              ProductService productService, 
                              IMapper mapper)
        {
            CartService = cartService;
            ProductService = productService;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // de verificat sa fie userul logat
            var cartProducts = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if(cartProducts == null)
            {
                return NotFound();
            }

            var model = new CartListVM()
            {
                CartList = cartProducts.Select(c => Mapper.Map<Cart, CartVM>(c)).ToList()
            };

            return View("../Cart/Cart", model);
        }

        [HttpGet]
        public IActionResult PlaceOrder()
        {
            var cartList = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if(cartList == null)
            {
                return NotFound();
            }

            CartService.PlaceOrder(cartList.ToList());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddProductToCart(int productId)
        {
            var product = ProductService.GetProductById(productId);
            if(product == null)
            {
                return NotFound();
            }

            var model = new CartVM()
            {
                ProductId = productId,
                ProductPrice = product.ProductPrice,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                QuantityBuy = 1
            };

            var modelMappedToEntity = Mapper.Map<Cart>(model);

            CartService.InsertToCart(modelMappedToEntity);

            return RedirectToAction("Index", "Cart"); 
        }

        [HttpGet]
        public IActionResult RemoveProductFromCart(int productId)
        {
            var cartToDelete = CartService.GetCartByProductIdAndUser(productId);

            CartService.DeleteProductFromCart(cartToDelete);

            return RedirectToAction("Index", "Cart");
        }
    }
}
