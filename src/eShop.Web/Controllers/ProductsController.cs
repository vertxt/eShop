using eShop.Shared.Parameters;
using eShop.Web.Interfaces;
using eShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _service;

        public ProductsController(IProductService productService)
        {
            _service = productService;
        }

        public async Task<IActionResult> Index(ProductParameters productParams)
        {
            try
            {
                var productsTask = _service.GetProductsAsync(productParams);
                var categoriesTask = _service.GetCategoriesAsync();

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
