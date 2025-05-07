using eShop.Shared.DTOs.Carts;

namespace eShop.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string userId, string sessionId);
        Task<CartDto> AddToCartAsync(string userId, string sessionId, AddToCartDto addToCartDto);
        Task<CartDto> UpdateCartItemAsync(string userId, string sessionId, UpdateCartItemDto updateCartDto);
        Task<CartDto> RemoveCartItemAsync(string userId, string sessionId, int cartItemId);
        Task<CartDto> ClearCartAsync(string userId, string sessionId);
        Task<CartSummaryDto> GetCartSummaryAsync(string userId, string sessionId);
    }
}