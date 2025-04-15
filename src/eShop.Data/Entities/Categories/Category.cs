using eShop.Data.Entities.Products;

namespace eShop.Data.Entities.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CategoryAttribute> Attributes { get; set; } = new List<CategoryAttribute>();
    }
}