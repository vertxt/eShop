namespace eShop.Shared.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class CreateCategoryAttributeDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}