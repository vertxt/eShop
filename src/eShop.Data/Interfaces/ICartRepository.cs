using eShop.Data.Entities.CartAggregate;

namespace eShop.Data.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartAsync(string? userId, string sessionId);
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<CartItem?> GetCartItemAsync(int cartItemId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId, int? productVariantId);
        Task<bool> AddCartItemAsync(CartItem cartItem);
        Task<bool> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> RemoveCartItemsAsync(CartItem cartItem);
        Task<bool> ClearCartAsync(int cartId);
    }
}