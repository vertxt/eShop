using System.Text.Json;
using eShop.Shared.DTOs.Carts;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CartService(IHttpClientFactory _httpClientFactory)
        {
            _httpClient = _httpClientFactory.CreateClient("API");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<CartDto> GetCartAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CartDto>("cart");
            return response ?? new CartDto();
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CartSummaryDto>("cart/summary");
            return response ?? new CartSummaryDto();
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            var response = await _httpClient.PostAsJsonAsync("cart/items", addToCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartDto)
        {
            var response = await _httpClient.PutAsJsonAsync("cart/items", updateCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> RemoveCartItemAsync(int cartItemId)
        {
            var response = await _httpClient.DeleteAsync($"cart/items/{cartItemId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> ClearCartAsync()
        {
            var response = await _httpClient.DeleteAsync("cart");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }
    }
}