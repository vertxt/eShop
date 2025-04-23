using eShop.Data.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByUuidAsync(string uuid);
        IQueryable<Product> GetByCategoryId(int id);
        Task<Product?> GetProductWithDetailsByIdAsync(int id);
        IQueryable<Product> GetProductsWithBasicDetails();

        // Variant-related methods
        Task<ProductVariant?> GetVariantByIdAsync(int variantId);
        Task<bool> AddVariantAsync(ProductVariant variant);
        Task<bool> UpdateVariantAsync(ProductVariant variant);
        Task<bool> DeleteVariantAsync(int variantId);

        // Image-related methods
        Task<ProductImage?> GetImageByIdAsync(int imageId);
        IQueryable<ProductImage> GetImagesByProductId(int productId);
        IQueryable<ProductImage> GetImagesByVariantId(int variantId);
        Task<bool> AddImageAsync(ProductImage image);
        Task<bool> UpdateImageAsync(ProductImage image);
        Task<bool> DeleteImageAsync(int imageId);
        Task<bool> SetMainImageAsync(int productId, int imageId);
        Task<bool> SetMainVariantImageAsync(int variantId, int imageId);

        // Attribute-related methods
        Task<ProductAttribute?> GetAttributeByIdAsync(int attributeId);
        Task<bool> AddAttributeAsync(ProductAttribute attribute);
        Task<bool> UpdateAttributeAsync(ProductAttribute attribute);
        Task<bool> DeleteAttributeAsync(int attributeId);
    }
}