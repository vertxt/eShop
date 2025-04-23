using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Category?> GetCategoryWithDetailsByIdAsync(int id)
        {
            return await _entities
                .Include(c => c.Products)
                .Include(c => c.Attributes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<CategoryAttribute> GetAllAttributes()
        {
            return _context.CategoryAttributes;
        }
    }
}