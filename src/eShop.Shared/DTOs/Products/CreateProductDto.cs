using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eShop.Shared.DTOs.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        public string ShortDescription { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Display(Name = "Has Variants")]
        public bool HasVariants { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int? QuantityInStock { get; set; }

        public List<CreateProductAttributeDto> Attributes { get; set; } = new List<CreateProductAttributeDto>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<ImageMetadataDto> ImageMetadata { get; set; } = new List<ImageMetadataDto>();
        public List<CreateProductVariantDto> Variants { get; set; } = new List<CreateProductVariantDto>();
    }

    public class ImageMetadataDto
    {
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateProductVariantDto
    {
        [Required(ErrorMessage = "Variant name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int? QuantityInStock { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class CreateProductAttributeDto
    {
        [Required(ErrorMessage = "Attribute is required")]
        public int AttributeId { get; set; }

        [Required(ErrorMessage = "Attribute value is required")]
        [StringLength(255, ErrorMessage = "Value cannot exceed 255 characters")]
        public string Value { get; set; }
    }
}
