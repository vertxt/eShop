using eShop.Data.Entities.CategoryAggregate;

namespace eShop.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithDetailsByIdAsync(int id);
        IQueryable<CategoryAttribute> GetAllAttributes();
    }
}