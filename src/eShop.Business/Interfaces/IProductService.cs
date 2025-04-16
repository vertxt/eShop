using eShop.Data.Entities.Products;

namespace eShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductByUuidAsync(string uuid);
        Task CreateProductAsync(Product newProduct);
        Task UpdateProductAsync(Product updatedProduct);
        Task DeleteProductAsync(int id);
    }
}