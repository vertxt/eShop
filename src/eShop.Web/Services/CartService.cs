using System.Text.Json;
using eShop.Shared.DTOs.Carts;
using eShop.Web.Extensions;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public CartService(IHttpClientFactory _httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = _httpClientFactory.CreateClient("API");
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<CartDto> GetCartAsync()
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.GetFromJsonAsync<CartDto>("cart");
            return response ?? new CartDto();
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync()
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.GetFromJsonAsync<CartSummaryDto>("cart/summary");
            return response ?? new CartSummaryDto();
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.PostAsJsonAsync("cart/items", addToCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartDto)
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.PutAsJsonAsync("cart/items", updateCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> RemoveCartItemAsync(int cartItemId)
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.DeleteAsync($"cart/items/{cartItemId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> ClearCartAsync()
        {
            var httpClient = await _httpClient.AddTokenAsync(_httpContextAccessor);
            var response = await httpClient.DeleteAsync("cart");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }
    }
}