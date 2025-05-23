using eShop.Data.Entities.CategoryAggregate;

namespace eShop.Data.Entities.ProductAggregate
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int AttributeId { get; set; }
        public CategoryAttribute Attribute { get; set; } = null!;
        public required string Value { get; set; }
    }
}