using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eShop.Web.Models;
using eShop.Web.Interfaces;
using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Categories;

namespace eShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService)
    {
        _logger = logger;
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var featuredProductsTask = _productService.GetFeaturedProductsAsync();
            var categoriesTask = _productService.GetCategoriesAsync();
            
            await Task.WhenAll(featuredProductsTask, categoriesTask);
            
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await featuredProductsTask ?? Enumerable.Empty<ProductDto>(),
                Categories = await categoriesTask ?? Enumerable.Empty<CategoryDto>()
            };
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching featured products");
            return View(new HomeViewModel());
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
