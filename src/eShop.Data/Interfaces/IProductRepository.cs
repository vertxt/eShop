using eShop.Data.Entities.Products;

namespace eShop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByUuidAsync(string uuid);
        IQueryable<Product> GetByCategoryId(int id);
    }
}