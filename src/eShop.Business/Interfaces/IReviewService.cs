using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;

namespace eShop.Business.Interfaces
{
    public interface IReviewService
    {
        Task<PagedList<ReviewDto>> GetProductReviewsAsync(int productId, PaginationParameters paginationParameters);
        Task<ReviewDto> AddAsync(int productId, string? userId, CreateReviewDto createReviewDto);
    }
}