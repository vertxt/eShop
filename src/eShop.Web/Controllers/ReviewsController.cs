using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    // Controller for AJAX fetching
    [Route("api/products")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(int productId, [FromQuery] PaginationParameters paginationParameters)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId, paginationParameters);
            return Ok(reviews);
        }

        // Controller to return a partial view (supporting product reviews pagination feature)
        [HttpGet("{productId}/reviews/html")]
        public async Task<IActionResult> GetReviewsHtml(int productId, [FromQuery] PaginationParameters paginationParameters)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId, paginationParameters);
            return PartialView("_ReviewsList", reviews.Items);
        }
    }
}