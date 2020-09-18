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
        public IActionResult PlaceOrder()
        {
            var cartList = CartService.GetAllCartProductsNotDeletedNotOrderPlaced();
            if (cartList == null)
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

            return View("AddProductToCart", model);
        }

        [HttpPost]
        public IActionResult AddProductToCart(CartVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var modelMappedToEntity = Mapper.Map<Cart>(model);

            CartService.InsertToCart(modelMappedToEntity);

            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public IActionResult RemoveProductFromCart(int productId)
        {
            var cartToDelete = CartService.GetCartByProductIdAndUser(productId);
            if(cartToDelete == null)
            {
                return NotFound();
            }
            // trebuie sa o las aici altfel am eroare de prea multe redirects ( intra in bucla cumva)
            CartService.DeleteProductFromCart(cartToDelete);

            return RedirectToAction("Index", "Cart");
        }
    }
}
