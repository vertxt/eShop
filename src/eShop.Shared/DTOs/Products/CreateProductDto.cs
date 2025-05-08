using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace eShop.Shared.DTOs.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public bool HasVariants { get; set; }
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
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInStock { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }
}
