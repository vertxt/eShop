
namespace eShop.Shared.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class UpdateCategoryAttributeDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}