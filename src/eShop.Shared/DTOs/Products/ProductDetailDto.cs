using System;
using System.Collections.Generic;
using eShop.Shared.DTOs.Products;

public class ProductDetailDto
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
    public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    public ICollection<ProductAttributeDto> Attributes { get; set; } = new List<ProductAttributeDto>();
    public ICollection<ProductReviewDto> Reviews { get; set; } = new List<ProductReviewDto>();
    public decimal AverageRating { get; set; }
}