using eShop.Shared.DTOs.Carts;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IApiClientWrapper _apiClient;

        public CartService(IApiClientWrapper apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CartDto> GetCartAsync()
        {
            return await _apiClient.GetAsync<CartDto>("cart", requiresAuth: true);
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync()
        {
            return await _apiClient.GetAsync<CartSummaryDto>("cart/summary", requiresAuth: true);
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            return await _apiClient.PostAsync<AddToCartDto, CartDto>("cart/items", addToCartDto, requiresAuth: true);
        }

        public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartDto)
        {
            return await _apiClient.PutAsync<UpdateCartItemDto, CartDto>("cart/items", updateCartDto, requiresAuth: true);
        }

        public async Task<CartDto> RemoveCartItemAsync(int cartItemId)
        {
            return await _apiClient.DeleteAsync<CartDto>($"cart/items/{cartItemId}", requiresAuth: true);
        }

        public async Task<CartDto> ClearCartAsync()
        {
            return await _apiClient.DeleteAsync<CartDto>("cart", requiresAuth: true);
        }
    }
}