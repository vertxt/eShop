using eShop.Data.Entities.Products;
using eShop.Data.Interfaces;

namespace eShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}