using eShop.Data.Entities.Categories;
using eShop.Data.Interfaces;

namespace eShop.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}