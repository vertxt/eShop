using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductDetailDto? Product { get; private set; }

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("API");
            Product = await httpClient.GetFromJsonAsync<ProductDetailDto>($"products/details/{id}");
            if (Product is null)
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}