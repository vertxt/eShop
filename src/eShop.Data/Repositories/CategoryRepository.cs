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

        public IQueryable<CategoryAttribute> GetAttributesByCategoryIdAsync(int categoryId)
        {
            return _context.CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId);
        }

        public async Task<CategoryAttribute?> GetAttributeByIdAsync(int id)
        {
            return await _context.CategoryAttributes.FindAsync(id);
        }

        public async Task<bool> AddAttributeAsync(CategoryAttribute categoryAttribute)
        {
            await _context.CategoryAttributes.AddAsync(categoryAttribute);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAttributeAsync(CategoryAttribute categoryAttribute)
        {
            _context.CategoryAttributes.Update(categoryAttribute);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAttributeAsync(int attributeId)
        {
            var attribute = await _context.CategoryAttributes.FindAsync(attributeId);
            if (attribute is null) return false;
            _context.CategoryAttributes.Remove(attribute);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}