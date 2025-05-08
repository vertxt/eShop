using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;

namespace eShop.Web.Interfaces
{
    public interface IProductService
    {
        Task<PagedList<ProductDto>?> GetProductsAsync(ProductParameters productParams);
        Task<IEnumerable<ProductDto>?> GetFeaturedProductsAsync();
        Task<List<CategoryDto>?> GetCategoriesAsync();
    }
}