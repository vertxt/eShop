using System.Text.Json;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public ProductsController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("API");
            var httpResponseMessage = await httpClient.GetAsync("products");
            httpResponseMessage.EnsureSuccessStatusCode();
            
            var json = await httpResponseMessage.Content.ReadAsStringAsync();
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<ProductDto>? products = JsonSerializer.Deserialize<List<ProductDto>>(json, jsonSerializerOptions);
            
            return View(products);
        }
    }
}