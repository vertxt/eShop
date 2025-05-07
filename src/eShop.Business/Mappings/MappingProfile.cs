using AutoMapper;
using eShop.Data.Entities.CartAggregate;
using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Entities.UserAggregate;
using eShop.Shared.DTOs.Carts;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Reviews;

namespace eShop.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product -> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.Images.FirstOrDefault(i => i.IsMain)!.Url ?? src.Images.FirstOrDefault()!.Url ?? string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.Reviews.Any() ? Math.Round(src.Reviews.Average(r => r.Rating), 2) : 0));

            // Product -> ProductDetailDto
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.AverageRating, opt =>
                    opt.MapFrom(src => src.Reviews.Any() ?
                        Math.Round(src.Reviews.Average(r => r.Rating), 2) : 0));

            // ProductImage -> ProductImageDto
            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.AltText, opt =>
                    opt.MapFrom(src => src.Product != null ? src.Product.Name : src.ProductVariant!.Name))
                .ForMember(dest => dest.DisplayOrder, opt =>
                    opt.MapFrom(src => src.DisplayOrder ?? 999));

            // ProductVariant -> ProductVariantDto
            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest => dest.Price, opt =>
                    opt.MapFrom(src => src.Price ?? src.Product.BasePrice))
                .ForMember(dest => dest.QuantityInStock, opt =>
                    opt.MapFrom(src => src.QuantityInStock));

            // ProductAttribute -> ProductAttributeDto
            CreateMap<ProductAttribute, ProductAttributeDto>()
                .ForMember(dest => dest.Name, opt =>
                    opt.MapFrom(src => src.Attribute.Name))
                .ForMember(dest => dest.DisplayName, opt =>
                    opt.MapFrom(src => src.Attribute.DisplayName ?? src.Attribute.Name));

            // CreateProductDto -> Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.Variants, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.Attributes, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            // CreateProductAttributeDto -> ProductAttribute
            CreateMap<CreateProductAttributeDto, ProductAttribute>();

            // CreateProductVariantDto -> ProductVariant
            CreateMap<CreateProductVariantDto, ProductVariant>();

            // UpdateProductVariantDto -> ProductVariant
            CreateMap<UpdateProductVariantDto, ProductVariant>();

            // UpdateProductDto -> Product
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.Variants, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.Attributes, opt => opt.Ignore()) // handle manually
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<CreateCategoryAttributeDto, CategoryAttribute>();
            CreateMap<Category, CategoryDetailDto>();
            CreateMap<CategoryAttribute, CategoryAttributeDto>();

            // Cart mappings
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt =>
                    opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImage,
                    opt => opt.MapFrom(src =>
                        src.Product.Images.FirstOrDefault(i => i.IsMain)!.Url
                        ?? src.Product.Images.FirstOrDefault()!.Url
                        ?? string.Empty))
                .ForMember(dest => dest.VariantId,
                    opt => opt.MapFrom(src => src.ProductVariantId))
                .ForMember(dest => dest.VariantName,
                    opt => opt.MapFrom(src => src.ProductVariant == null ? string.Empty : src.ProductVariant.Name))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.ProductVariant == null ? src.Product.BasePrice : src.ProductVariant.Price))
                .ForMember(dest => dest.Subtotal,
                    opt => opt.MapFrom(src => (src.ProductVariant == null ? src.Product.BasePrice : src.ProductVariant.Price) * src.Quantity));

            CreateMap<Cart, CartSummaryDto>()
                .ForMember(dest => dest.TotalItems,
                    opt => opt.MapFrom(src => src.CartItems.Sum(ci => ci.Quantity)))
                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.CartItems.Sum(
                        ci => ci.Quantity * (ci.ProductVariant == null ? ci.Product.BasePrice : ci.ProductVariant.Price)
                    )));

            CreateMap<AddToCartDto, CartItem>()
                .ForMember(dest => dest.ProductVariantId,
                    opt => opt.MapFrom(src => src.VariantId));

            CreateMap<UpdateCartItemDto, CartItem>();

            // Product Review
            // Review -> ReviewDto
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Reviewer, opt =>
                    opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => Math.Round(src.Rating, 2)));
            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.CreatedDate, opt =>
                    opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}