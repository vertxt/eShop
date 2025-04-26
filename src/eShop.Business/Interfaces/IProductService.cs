using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace eShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<PagedList<ProductDto>> GetAllAsync(ProductParameters productParams);
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> GetByUuidAsync(string uuid);
        Task<ProductDetailDto> GetDetailByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task DeleteAsync(int id);

        // Variant methods
        Task<ProductVariantDto> GetVariantByIdAsync(int variantId);
        Task<int> AddVariantAsync(int productId, CreateProductVariantDto createProductVariantDto);
        Task<bool> UpdateVariantAsync(int variantId, UpdateProductVariantDto updateProductVariantDto);
        Task<bool> DeleteVariantAsync(int variantId);
        
        // Image methods
        Task<int> AddImageAsync(CreateProductImageDto createProductImageDto, int? productId = null, int? variantId = null);
        Task<int> AddProductImageAsync(int productId, CreateProductImageDto createProductImageDto);
        Task<int> AddVariantImageAsync(int variantId, CreateProductImageDto createProductImageDto);
        Task<bool> UpdateProductImageAsync(int imageId, UpdateProductImageDto updateProductImageDto);
        Task<bool> DeleteProductImageAsync(int imageId);
        
        // Attribute methods
        Task<ProductAttributeDto> GetAttributeByIdAsync(int attributeId);
        Task<int> AddAttributeAsync(int productId, CreateProductAttributeDto createProductAttributeDto);
        Task<bool> UpdateAttributeAsync(int attributeId, UpdateProductAttributeDto updateProductAttributeDto);
        Task<bool> DeleteAttributeAsync(int attributeId);
        Task<IEnumerable<ProductAttributeDto>> GetAttributesByProductIdAsync(int productId);
    }
}