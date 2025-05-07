using eShop.Shared.DTOs.Carts;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Components
{
    public class OrderSummaryViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public OrderSummaryViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CartSummaryDto? summary = null)
        {
            if (summary == null)
            {
                summary = await _cartService.GetCartSummaryAsync();
            }

            return View(summary);
        }
    }
}