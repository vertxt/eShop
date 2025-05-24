using eShop.Shared.DTOs.Dashboard;
using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Reviews;

namespace eShop.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<CategoryProductCountDto>> GetProductsByCategoryDistributionAsync(int? limit = null);
        Task<IEnumerable<RatingDistributionDto>> GetRatingDistributionAsync();
        Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int? count = 5);
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int? threshold = 10, int? count = 5);
        Task<IEnumerable<ProductDto>> GetLatestFeaturedProductsAsync(int? count = 3);
    }
}