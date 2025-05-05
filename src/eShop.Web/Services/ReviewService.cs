using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;

namespace eShop.Web.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IApiClientWrapper _apiClientWrapper;
        public ReviewService(IApiClientWrapper apiClientWrapper)
        {
            _apiClientWrapper = apiClientWrapper;
        }

        public async Task<PagedList<ReviewDto>> GetProductReviewsAsync(int productId, PaginationParameters paginationParameters)
        {
            var queryBuilder = new QueryBuilder
            {
                { "PageNumber", paginationParameters.PageNumber.ToString() },
                { "PageSize", paginationParameters.PageSize.ToString() }
            };

            var queryString = queryBuilder.ToQueryString();

            return await _apiClientWrapper.GetAsync<PagedList<ReviewDto>>($"products/{productId}/reviews{queryString}");
        }

        public async Task<ReviewDto> AddReviewAsync(int productId, CreateReviewDto createReviewDto)
        {
            return await _apiClientWrapper.PostAsync<CreateReviewDto, ReviewDto>($"products/{productId}/reviews", createReviewDto, requiresAuth: true);
        }
    }
}