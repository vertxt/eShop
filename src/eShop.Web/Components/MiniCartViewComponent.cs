using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Components
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public MiniCartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartSummary = await _cartService.GetCartSummaryAsync();
            return View(cartSummary);
        }
    }
}