using eShop.Shared.Parameters;
using eShop.Web.Interfaces;
using eShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Components
{
    public class ProductReviewsViewComponent : ViewComponent
    {
        private readonly IReviewService _reviewService;

        public ProductReviewsViewComponent(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId, PaginationParameters paginationParameters, decimal averageRating, int count)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId, paginationParameters);
            var viewModel = new ProductReviewsViewModel
            {
                Reviews = reviews,
                AverageRating = averageRating,
                Count = count,
                ProductId = productId,
            };

            return View(viewModel);
        }
    }
}