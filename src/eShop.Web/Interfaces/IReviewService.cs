using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;

namespace eShop.Web.Interfaces
{
    public interface IReviewService
    {
        Task<PagedList<ReviewDto>> GetProductReviewsAsync(int productId, PaginationParameters paginationParameters);
        Task<ReviewDto> AddReviewAsync(int productId, CreateReviewDto createReviewDto);
    }
}