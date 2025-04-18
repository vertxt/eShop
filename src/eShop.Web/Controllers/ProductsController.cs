using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using eShop.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public ProductsController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index(ProductParameters productParams)
        {
            var httpClient = _httpClientFactory.CreateClient("API");
            var queryString = productParams.ToQueryString();
            var pagedProducts = await httpClient.GetFromJsonAsync<PagedList<ProductDto>>($"products?{queryString}");

            return View(pagedProducts);
        }
    }
}