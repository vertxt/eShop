using eShop.Shared.DTOs.Carts;

namespace eShop.Web.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync();
        Task<CartSummaryDto> GetCartSummaryAsync();
        Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto);
        Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
        Task<CartDto> RemoveCartItemAsync(int cartItemId);
        Task<CartDto> ClearCartAsync();
    }
}