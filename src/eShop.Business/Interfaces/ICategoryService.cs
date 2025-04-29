using eShop.Shared.DTOs.Categories;

namespace eShop.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDetailDto> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<CategoryAttributeDto>> GetAttributesByCategoryIdAsync(int categoryId);
        Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task DeleteAsync(int id);

        /* UNUSED METHODS (commented out) */
        // Task<int> AddAttributeAsync(int categoryId, CreateCategoryAttributeDto createCategoryAttributeDto);
        // Task<bool> UpdateAttributeAsync(int attributeId, UpdateCategoryAttributeDto updateCategoryAttributeDto);
        // Task<bool> DeleteAttributeAsync(int attributeId);
    }
}