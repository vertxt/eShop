using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Entities.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public required string Name { get; set; }
        public int QuantityInStock { get; set; }
        [Precision(14, 2)]
        public decimal? Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}