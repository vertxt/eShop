using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        private string GetOrCreateSessionId()
        {
            var sessionId = Request.Cookies["CartSessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                };

                Response.Cookies.Append("CartSessionId", sessionId, cookieOptions);
            }

            return sessionId;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var cart = await _cartService.GetCartAsync(GetUserId(), GetOrCreateSessionId());
            return Ok(cart);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<CartSummaryDto>> GetCartSummary()
        {
            var summary = await _cartService.GetCartSummaryAsync(GetUserId(), GetOrCreateSessionId());
            return Ok(summary);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddToCart(AddToCartDto addToCartDto)
        {
            var cart = await _cartService.AddToCartAsync(GetUserId(), GetOrCreateSessionId(), addToCartDto);
            return Ok(cart);
        }

        [HttpPut("items")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(UpdateCartItemDto updateCartDto)
        {
            var cart = await _cartService.UpdateCartItemAsync(GetUserId(), GetOrCreateSessionId(), updateCartDto);
            return Ok(cart);
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult<CartDto>> RemoveCartItem(int id)
        {
            var cart = await _cartService.RemoveCartItemAsync(GetUserId(), GetOrCreateSessionId(), id);
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<ActionResult<CartDto>> ClearCart()
        {
            var cart = await _cartService.ClearCartAsync(GetUserId(), GetOrCreateSessionId());
            return Ok(cart);
        }
    }
}