namespace eShop.Shared.DTOs.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
    }

    public class UpdateProductImageDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateProductVariantDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInStock { get; set; }
    }

    public class UpdateProductAttributeDto
    {
        public string Value { get; set; }
    }
}