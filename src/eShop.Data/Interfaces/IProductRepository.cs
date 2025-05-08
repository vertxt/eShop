using eShop.Data.Entities.ProductAggregate;

namespace eShop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        // Query
        IQueryable<Product> GetFeaturedProducts();
        Task<Product?> GetByIdWithBasicDetailsAsync(int id);
        Task<Product?> GetByUuidAsync(string uuid);
        Task<Product?> GetByIdWithDetailsAsync(int id);

        // CRUD operations
        void RemoveImageAsync(ProductImage image);

        Task DeleteProductAsync(int productId);
    }
}