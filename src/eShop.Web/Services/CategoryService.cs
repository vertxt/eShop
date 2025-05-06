using eShop.Shared.DTOs.Categories;
using eShop.Web.Interfaces;

namespace eShop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApiClientWrapper _apiClientWrapper;

        public CategoryService(IApiClientWrapper apiClientWrapper)
        {
            _apiClientWrapper = apiClientWrapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _apiClientWrapper.GetAsync<List<CategoryDto>>("categories");
        }
    }
}