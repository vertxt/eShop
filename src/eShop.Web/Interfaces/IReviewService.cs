using eShop.Shared.DTOs.Reviews;

namespace eShop.Web.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId);
        Task<ReviewDto> AddReviewAsync(int productId, CreateReviewDto createReviewDto);
    }
}