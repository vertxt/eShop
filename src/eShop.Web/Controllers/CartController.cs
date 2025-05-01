using eShop.Shared.DTOs.Carts;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _service;

        public CartController(ICartService cartService)
        {
            _service = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _service.GetCartAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int? variantId, int quantity = 1)
        {
            var addToCartDto = new AddToCartDto
            {
                ProductId = productId,
                VariantId = variantId,
                Quantity = quantity
            };

            await _service.AddToCartAsync(addToCartDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            var updateCartItemDto = new UpdateCartItemDto
            {
                Id = cartItemId,
                Quantity = quantity,
            };

            await _service.UpdateCartItemAsync(updateCartItemDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            await _service.RemoveCartItemAsync(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            await _service.ClearCartAsync();
            return RedirectToAction("Index");
        }
    }
}