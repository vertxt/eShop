using System.Text.Json;
using eShop.Shared.DTOs.Reviews;
using eShop.Web.Extensions;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;
        public ReviewService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ReviewDto>>($"products/{productId}/reviews");
            return response ?? new List<ReviewDto>();
        }

        public async Task<ReviewDto> AddReviewAsync(int productId, CreateReviewDto createReviewDto)
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.PostAsJsonAsync($"products/{productId}/reviews", createReviewDto);
            return await response.Content.ReadFromJsonAsync<ReviewDto>(_jsonOptions) ?? new ReviewDto();
        }
    }
}