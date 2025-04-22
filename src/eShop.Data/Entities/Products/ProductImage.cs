namespace eShop.Data.Entities.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public int? DisplayOrder { get; set; }
    }
}