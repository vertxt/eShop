using eShop.Data.Entities.Users;

namespace eShop.Data.Entities.Carts
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? SessionId { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}