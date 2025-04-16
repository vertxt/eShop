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
}