using eShop.Data.Entities.ProductAggregate;

namespace eShop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        // Query
        IQueryable<Product> GetAllWithBasicDetails();
        Task<Product?> GetByUuidAsync(string uuid);
        Task<Product?> GetByIdWithDetailsAsync(int id);

        // CRUD operations
        void RemoveImageAsync(ProductImage image);

        /* UNUSED METHODS (commented out) */
        // Variant-related methods
        // Task<ProductVariant?> GetVariantByIdAsync(int variantId);
        // Task<bool> AddVariantAsync(ProductVariant variant);
        // Task<bool> UpdateVariantAsync(ProductVariant variant);
        // Task<bool> DeleteVariantAsync(int variantId);

        // Image-related methods
        // Task<ProductImage?> GetImageByIdAsync(int imageId);
        // IQueryable<ProductImage> GetImagesByProductId(int productId);
        // IQueryable<ProductImage> GetImagesByVariantId(int variantId);
        // Task<bool> AddImageAsync(ProductImage image);
        // Task<bool> UpdateImageAsync(ProductImage image);
        // Task<bool> DeleteImageAsync(int imageId);
        // Task<bool> SetMainImageAsync(int productId, int imageId);
        // Task<bool> SetMainVariantImageAsync(int variantId, int imageId);

        // Attribute-related methods
        // Task<ProductAttribute?> GetAttributeByIdAsync(int attributeId);
        // Task<bool> AddAttributeAsync(ProductAttribute attribute);
        // Task<bool> UpdateAttributeAsync(ProductAttribute attribute);
        // Task<bool> DeleteAttributeAsync(int attributeId);
    }
}