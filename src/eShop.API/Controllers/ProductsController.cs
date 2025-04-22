using AutoMapper;
using eShop.API.Extensions;
using eShop.Business.Interfaces;
using eShop.Data.Entities.Products;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProductsAsync([FromQuery]ProductParameters productParams)
        {
            var pagedProducts = await _productService.GetProductsAsync(productParams);

            return Ok(pagedProducts.MapItems<Product, ProductDto>(_mapper));
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            ProductDto productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }

        [HttpGet("uuid/{uuid}")]
        public async Task<ActionResult<ProductDto>> GetProductByUuidAsync(string uuid)
        {
            var product = await _productService.GetProductByUuidAsync(uuid);
            if (product is null)
            {
                return NotFound();
            }

            ProductDto productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(createProductDto);

                await _productService.CreateProductAsync(product);

                var addedProduct = await _productService.GetProductByIdAsync(product.Id);
                var addedProductDto = _mapper.Map<ProductDto>(addedProduct);

                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = addedProductDto.Id }, addedProductDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                return StatusCode(500, "An error occured while processing your request");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);

                if (existingProduct is null)
                {
                    return NotFound();
                }

                _mapper.Map(updateProductDto, existingProduct);

                await _productService.UpdateProductAsync(existingProduct);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting product");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}