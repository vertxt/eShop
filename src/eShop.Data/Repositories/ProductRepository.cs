using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        // Query
        public IQueryable<Product> GetFeaturedProducts()
        {
            return _entities.Where(p => p.IsFeatured);
        }

        public Task<Product?> GetByIdWithBasicDetailsAsync(int id)
        {
            return _entities
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByUuidAsync(string uuid)
        {
            return await _entities.FirstAsync(p => p.Uuid == uuid);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _entities
                .AsSplitQuery()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.Images)
                .Include(p => p.Attributes)
                    .ThenInclude(pa => pa.Attribute)
                .Include(p => p.Reviews)
                    .ThenInclude(pr => pr.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // CRUD operations
        public void RemoveImageAsync(ProductImage image)
        {
            _context.ProductImages.Remove(image);
        }

        // Temporary solution to avoid violating the CartItems_Products foreign key constraint
        public async Task DeleteProductAsync(int productId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.ProductId == productId)
                .ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
        }
    }
}
