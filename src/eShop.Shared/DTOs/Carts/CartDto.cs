using System.Collections.Generic;

namespace eShop.Shared.DTOs.Carts
{
    public class CartDto
    {
        public int Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public CartSummaryDto Summary { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; }
        public int? VariantId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class CartSummaryDto
    {
        public int TotalItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}