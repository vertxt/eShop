using System.Collections.Generic;

namespace eShop.Shared.DTOs.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public bool HasVariants { get; set; }
        public int? QuantityInStock { get; set; }
        public List<CreateProductAttributeDto> Attributes { get; set; } = new List<CreateProductAttributeDto>();
    }

    public class CreateProductImageDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateProductVariantDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInStock { get; set; }
    }

    public class CreateProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }

    public class CreateProductReviewDto
    {
        public decimal Rating { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
