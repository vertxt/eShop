using AutoMapper;
using eShop.Data.Entities.Products;
using eShop.Shared.DTOs.Products;

namespace eShop.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Attributes, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Attributes, opt => opt.Ignore());

            // Nested entity mappings
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<CreateProductImageDto, ProductImage>();
            
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<CreateProductVariantDto, ProductVariant>();
            
            CreateMap<ProductAttribute, ProductAttributeDto>();
            CreateMap<CreateProductAttributeDto, ProductAttribute>();
        }

    }
}