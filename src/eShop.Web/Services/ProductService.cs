using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;

namespace eShop.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IApiClientWrapper _apiClientWrapper;

        public ProductService(IApiClientWrapper apiClientWrapper)
        {
            _apiClientWrapper = apiClientWrapper;
        }

        public async Task<PagedList<ProductDto>?> GetProductsAsync(ProductParameters productParams)
        {
            var queryString = BuildProductsQueryString(productParams);

            return await _apiClientWrapper.GetAsync<PagedList<ProductDto>>($"products{queryString}", requiresAuth: false);
        }

        public async Task<IEnumerable<ProductDto>?> GetFeaturedProductsAsync()
        {
            return await _apiClientWrapper.GetAsync<List<ProductDto>>("products/featured", requiresAuth: false);
        }

        public async Task<List<CategoryDto>?> GetCategoriesAsync()
        {
            return await _apiClientWrapper.GetAsync<List<CategoryDto>>("categories");
        }

        private QueryString BuildProductsQueryString(ProductParameters productParams)
        {
            var queryBuilder = new QueryBuilder();

            if (!string.IsNullOrEmpty(productParams.SearchTerm))
                queryBuilder.Add("SearchTerm", productParams.SearchTerm);

            if (productParams.MinPrice.HasValue)
                queryBuilder.Add("MinPrice", productParams.MinPrice.Value.ToString());

            if (productParams.MaxPrice.HasValue)
                queryBuilder.Add("MaxPrice", productParams.MaxPrice.Value.ToString());

            if (productParams.CategoryIds is not null)
            {
                foreach (var categoryId in productParams.CategoryIds)
                {
                    queryBuilder.Add("CategoryIds", categoryId.ToString());
                }
            }

            queryBuilder.Add("PageNumber", productParams.PageNumber.ToString());
            queryBuilder.Add("PageSize", productParams.PageSize.ToString());
            queryBuilder.Add("SortBy", productParams.SortBy);

            return queryBuilder.ToQueryString();
        }
    }
}