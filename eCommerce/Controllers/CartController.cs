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
            var cartProducts = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if (cartProducts == null)
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
        public IActionResult JsCartModalView()
        {
            var cartProducts = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if (cartProducts == null)
            {
                return NotFound();
            }

            var model = new CartListVM()
            {
                CartList = cartProducts.Select(c => Mapper.Map<Cart, CartVM>(c)).ToList()
            };

            return Json(new { 
                cartModelList = model.CartList,
                flag = true
            });
        }

        [HttpGet]
        public IActionResult PlaceOrder(int deliveryLocationId)
        {
            var cartList = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if (cartList == null)
            {
                return NotFound();
            }

            CartService.PlaceOrder(cartList.ToList(), deliveryLocationId);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddProductToCart(int productId)
        {
            // de verificat daca nu e deja bagata
            var product = ProductService.GetProductById(productId);
            if (product == null)
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

        [HttpPost]
        public IActionResult RemoveProductFromCart(int productId)
        {
            var cartToDelete = CartService.GetCartByProductIdAndUser(productId);
            if(cartToDelete == null)
            {
                return NotFound();
            }
            
            CartService.DeleteProductFromCart(cartToDelete);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult JsRemoveProductFromCart(int productId)
        {
            var cartToDelete = CartService.GetCartByProductIdAndUser(productId);
            if(cartToDelete == null)
            {
                return NotFound();
            }
            // trebuie sa o las aici altfel am eroare de prea multe redirects ( intra in bucla cumva)
            CartService.DeleteProductFromCart(cartToDelete);

            return Json(new{
                flag = true
            });
        }
    }
}
