using System.Text.Json;
using eShop.Shared.DTOs.Reviews;
using eShop.Web.Extensions;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IApiClientWrapper _apiClientWrapper;
        public ReviewService(IApiClientWrapper apiClientWrapper)
        {
            _apiClientWrapper = apiClientWrapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            return await _apiClientWrapper.GetAsync<List<ReviewDto>>($"products/{productId}/reviews");
        }

        public async Task<ReviewDto> AddReviewAsync(int productId, CreateReviewDto createReviewDto)
        {
            return await _apiClientWrapper.PostAsync<CreateReviewDto, ReviewDto>($"products/{productId}/reviews", createReviewDto, requiresAuth: true);
        }
    }
}