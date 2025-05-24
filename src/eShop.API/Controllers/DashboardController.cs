using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Dashboard;
using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<DashboardSummaryDto> GetDashboardSummary()
        {
            return await _dashboardService.GetSummaryAsync();
        }

        [HttpGet("products-by-category")]
        public async Task<IEnumerable<CategoryProductCountDto>> GetProductsByCategoryDistribution(int? limit = null)
        {
            return await _dashboardService.GetProductsByCategoryDistributionAsync(limit);
        }

        [HttpGet("rating-distribution")]
        public async Task<IEnumerable<RatingDistributionDto>> GetRatingDistribution()
        {
            return await _dashboardService.GetRatingDistributionAsync();
        }

        [HttpGet("recent-reviews")]
        public async Task<IEnumerable<ReviewDto>> GetRecentReviews(int? count = 5)
        {
            return await _dashboardService.GetRecentReviewsAsync(count);
        }

        [HttpGet("low-stock-products")]
        public async Task<IEnumerable<ProductDto>> GetLowStockProducts(int? threshold = 10, int? count = 5)
        {
            return await _dashboardService.GetLowStockProductsAsync(threshold, count);
        }

        [HttpGet("latest-featured-products")]
        public async Task<IEnumerable<ProductDto>> GetLatestFeaturedProducts(int? count = 3)
        {
            return await _dashboardService.GetLatestFeaturedProductsAsync(count);
        }
    }
}