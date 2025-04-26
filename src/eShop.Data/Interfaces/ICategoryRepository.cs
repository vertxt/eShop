using eShop.Data.Entities.CategoryAggregate;

namespace eShop.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithDetailsByIdAsync(int id);
        IQueryable<CategoryAttribute> GetAttributesByCategoryIdAsync(int categoryId);
        Task<CategoryAttribute?> GetAttributeByIdAsync(int id);
        Task<bool> AddAttributeAsync(CategoryAttribute categoryAttribute);
        Task<bool> UpdateAttributeAsync(CategoryAttribute categoryAttribute);
        Task<bool> DeleteAttributeAsync(int attributeId);
    }
}