using System.Collections.Generic;
using eShop.Shared.DTOs.Products;

namespace eShop.Shared.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class CategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CategoryAttributeDto> Attributes { get; set; } = new List<CategoryAttributeDto>();
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
    
    public class CategoryAttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}