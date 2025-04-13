namespace eShop.Data.Entities.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public required string VariantName { get; set; }
        public int QuantityInStock { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}