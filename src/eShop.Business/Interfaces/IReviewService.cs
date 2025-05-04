using eShop.Shared.DTOs.Reviews;

namespace eShop.Business.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId);
        Task<ReviewDto> AddAsync(int productId, string? userId, CreateReviewDto createReviewDto);
    }
}