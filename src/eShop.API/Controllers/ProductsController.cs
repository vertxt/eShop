using eShop.Business.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
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
        public async Task<ActionResult<PagedList<ProductDto>>> GetAll([FromQuery] ProductParameters productParameters)
        {
            var products = await _productService.GetAllAsync(productParameters);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("{uuid:guid}")]
        public async Task<ActionResult<ProductDto>> GetByUuid(string uuid)
        {
            var product = await _productService.GetByUuidAsync(uuid);
            return Ok(product);
        }

        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> GetDetailsById(int id)
        {
            var product = await _productService.GetDetailByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateProductDto createProductDto)
        {
            var result = await _productService.CreateAsync(createProductDto);

            return CreatedAtAction
            (
                actionName: "GetById",
                routeValues: new { id = result.Id },
                value: result
            );
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            _ = await _productService.UpdateAsync(id, updateProductDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}