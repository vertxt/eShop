using System.Security.Claims;
using eShop.Business.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService reviewService)
        {
            _service = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ReviewDto>>> Get(int productId, [FromQuery] PaginationParameters paginationParameters)
        {
            var reviews = await _service.GetProductReviewsAsync(productId, paginationParameters);
            return Ok(reviews);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Post(int productId, CreateReviewDto createReviewDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = await _service.AddAsync(productId, userId, createReviewDto);
            return CreatedAtAction(nameof(Get), new { productId }, review);
        }
    }
}