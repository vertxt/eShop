using eShop.Data.Entities.Products;
using eShop.Shared.Common.Pagination;
using eShop.Shared.Parameters;

namespace eShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<PagedList<Product>> GetProductsAsync(ProductParameters productParams);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductByUuidAsync(string uuid);
        Task CreateProductAsync(Product newProduct);
        Task UpdateProductAsync(Product updatedProduct);
        Task DeleteProductAsync(int id);
    }
}