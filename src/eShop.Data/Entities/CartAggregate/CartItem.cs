using eShop.Data.Entities.ProductAggregate;

namespace eShop.Data.Entities.CartAggregate
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int? ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int Quantity { get; set; }
    }
}