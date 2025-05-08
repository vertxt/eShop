using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;

namespace eShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<PagedList<ProductDto>> GetAllAsync(ProductParameters productParams);
        Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> GetByIdWithBasicDetailsAsync(int id);
        Task<ProductDto> GetByUuidAsync(string uuid);
        Task<ProductDetailDto> GetDetailByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task DeleteAsync(int id);
    }
}