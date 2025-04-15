using eShop.API.DTOs;
using eShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDtos = products.Select(p => new ProductDto { Name = p.Name, Description = p.Description, BasePrice = p.BasePrice });
            return Ok(productDtos);
        }
    }
}