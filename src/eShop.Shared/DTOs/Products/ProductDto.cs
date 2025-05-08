using System;
using System.Collections.Generic;
using eShop.Shared.DTOs.Reviews;

namespace eShop.Shared.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string ShortDescription { get; set; }
        public string MainImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ReviewCount { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public ICollection<ProductAttributeDto> Attributes { get; set; } = new List<ProductAttributeDto>();
        // public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
        public int ReviewCount { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string SKU { get; set; }
        public bool isActive { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }

    public class ProductAttributeDto
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
