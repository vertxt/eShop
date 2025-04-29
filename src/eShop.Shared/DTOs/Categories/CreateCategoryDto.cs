using System.Collections.Generic;

namespace eShop.Shared.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreateCategoryAttributeDto> Attributes { get; set; } = new List<CreateCategoryAttributeDto>();
    }
    
    public class CreateCategoryAttributeDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}