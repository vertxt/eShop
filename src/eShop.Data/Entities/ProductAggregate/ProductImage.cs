namespace eShop.Data.Entities.ProductAggregate
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public required string Url { get; set; }
        public string? PublicId { get; set; }
        public bool IsMain { get; set; }
        public int? DisplayOrder { get; set; }
    }
}