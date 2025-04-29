using System.Collections.Generic;

namespace eShop.Shared.DTOs.Products
{
    public class UpdateProductDto : CreateProductDto
    {
        public new List<UpdateProductVariantDto> Variants { get; set; } = new List<UpdateProductVariantDto>();
        public List<int> ExistingImageIds { get; set; } = new List<int>();
        public List<ImageMetadataDto> ExistingImageMetadata { get; set; } = new List<ImageMetadataDto>();
    }

    public class UpdateProductVariantDto : CreateProductVariantDto
    {
        public int? Id { get; set; }
    }
}