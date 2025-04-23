using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Entities.ProductAggregate
{
    public class Product
    {
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        [Precision(14, 2)]
        public required decimal BasePrice { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}