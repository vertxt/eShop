using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductImageController(IProductService productService)
        {
            _productService = productService;
        }

        // Product Images
        [HttpPost("products/{productId}/images")]
        public async Task<ActionResult> AddProductImage(int productId, CreateProductImageDto createProductImageDto)
        {
            var imageId = await _productService.AddProductImageAsync(productId, createProductImageDto);
            return CreatedAtAction(nameof(GetProductImage), new { productId, imageId }, null);
        }

        [HttpGet("products/{productId}/images/{imageId}")]
        public async Task<ActionResult> GetProductImage(int productId, int imageId)
        {
            // To be implemented soon
            return Ok();
        }

        [HttpPut("images/{imageId}")]
        public async Task<ActionResult> UpdateImage(int imageId, UpdateProductImageDto updateProductImageDto)
        {
            var result = await _productService.UpdateProductImageAsync(imageId, updateProductImageDto);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500, "Failed to update image");
        }

        [HttpDelete("images/{imageId}")]
        public async Task<ActionResult> DeleteImage(int imageId)
        {
            var result = await _productService.DeleteProductImageAsync(imageId);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500, "Failed to delete image");
        }

        // Variant Images
        [HttpPost("variants/{variantId}/images")]
        public async Task<ActionResult> AddVariantImage(int variantId, CreateProductImageDto createProductImageDto)
        {
            var imageId = await _productService.AddVariantImageAsync(variantId, createProductImageDto);
            return CreatedAtAction(nameof(GetVariantImage), new { variantId, imageId }, null);
        }

        [HttpGet("variants/{variantId}/images/{imageId}")]
        public async Task<ActionResult> GetVariantImage(int variantId, int imageId)
        {
            // To be implemented soon
            return Ok();
        }
    }
}