using eShop.Data.Entities.Products;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
        public async Task<Product?> GetByUuidAsync(string uuid)
        {
            return await _entities.FirstAsync(p => p.Uuid == uuid);
        }

        public IQueryable<Product> GetByCategoryId(int id)
        {
            return _entities.Where(e => e.CategoryId == id);
        }
    }
}