using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using eShop.Web.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index(ProductParameters productParams)
        {
            try
            {
                // construct query string
                var httpClient = _httpClientFactory.CreateClient("API");

                var queryBuilder = new QueryBuilder();

                if (!string.IsNullOrEmpty(productParams.SearchTerm))
                    queryBuilder.Add("SearchTerm", productParams.SearchTerm);

                if (productParams.MinPrice.HasValue)
                    queryBuilder.Add("MinPrice", productParams.MinPrice.Value.ToString());

                if (productParams.MaxPrice.HasValue)
                    queryBuilder.Add("MaxPrice", productParams.MaxPrice.Value.ToString());

                if (productParams.InStock.HasValue)
                    queryBuilder.Add("InStock", productParams.InStock.Value.ToString());

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

                // Use extension method to construct query string
                // var queryString = productParams.ToQueryString();

                // fetch paged products
                var productsTask = httpClient.GetFromJsonAsync<PagedList<ProductDto>>($"products{queryBuilder.ToQueryString()}");
                var categoriesTask = httpClient.GetFromJsonAsync<List<CategoryDto>>("categories");

                await Task.WhenAll(productsTask, categoriesTask);

                var productCatalogViewModel = new ProductCatalogViewModel
                {
                    Products = await productsTask,
                    CurrentParams = productParams,
                    CatalogFilterViewModel = new CatalogFilterViewModel
                    {
                        Categories = await categoriesTask,
                    },
                    CatalogControlsViewModel = new(),
                };

                return View(productCatalogViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                throw;
            }
        }
    }
}
