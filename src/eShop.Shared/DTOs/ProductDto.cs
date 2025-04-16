using System;
using System.Collections.Generic;

namespace eShop.Shared.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<ProductImageDto> Images { get; set; }
        public ICollection<ProductVariantDto> Variants { get; set; }
        public ICollection<ProductAttributeDto> Attributes { get; set; }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInStock { get; set; }
    }

    public class ProductAttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}