using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop.Shared.DTOs.Categories
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public List<CreateCategoryAttributeDto> Attributes { get; set; } = new List<CreateCategoryAttributeDto>();
    }

    public class CreateCategoryAttributeDto
    {
        [Required(ErrorMessage = "Attribute name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters")]
        public string DisplayName { get; set; }
    }
}