using System.Net.Http.Headers;
using System.Text.Json;
using eShop.Shared.DTOs.Carts;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Authentication;

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

        private async Task AttachBearerTokenAsync()
        {
            var token = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");
            var id_token = await _httpContextAccessor.HttpContext!.GetTokenAsync("id_token");

            Console.WriteLine("Access Token here: " + token);
            Console.WriteLine("Token is null or empty: " + string.IsNullOrEmpty(token));
            Console.WriteLine("Id token: " + id_token);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<CartDto> GetCartAsync()
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.GetFromJsonAsync<CartDto>("cart");
            return response ?? new CartDto();
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync()
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.GetFromJsonAsync<CartSummaryDto>("cart/summary");
            return response ?? new CartSummaryDto();
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.PostAsJsonAsync("cart/items", addToCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartDto)
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.PutAsJsonAsync("cart/items", updateCartDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> RemoveCartItemAsync(int cartItemId)
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.DeleteAsync($"cart/items/{cartItemId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }

        public async Task<CartDto> ClearCartAsync()
        {
            await AttachBearerTokenAsync();
            var response = await _httpClient.DeleteAsync("cart");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>(_jsonOptions) ?? new CartDto();
        }
    }
}