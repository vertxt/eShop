using eShop.Shared.DTOs.Categories;

namespace eShop.Web.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
    }
}