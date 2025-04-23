using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products/{productId}/variants")]
public class ProductVariantsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductVariantsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{variantId}")]
    public async Task<ActionResult<ProductVariantDto>> GetVariant(int variantId)
    {
        var variant = await _productService.GetVariantByIdAsync(variantId);

        return Ok(variant);
    }

    [HttpPost]
    public async Task<ActionResult<ProductVariantDto>> CreateVariant(int productId, CreateProductVariantDto createProductVariantDto)
    {
        var variantId = await _productService.AddVariantAsync(productId, createProductVariantDto);
        var createdVariant = await _productService.GetVariantByIdAsync(variantId);

        return CreatedAtAction(nameof(GetVariant), new { productId, variantId }, createdVariant);
    }

    [HttpPut("{variantId}")]
    public async Task<IActionResult> UpdateVariant(int variantId, UpdateProductVariantDto updateProductVariantDto)
    {
        var result = await _productService.UpdateVariantAsync(variantId, updateProductVariantDto);
        if (result)
        {
            return NoContent();
        }

        return StatusCode(500, "Failed to update variant");
    }

    [HttpDelete("{variantId}")]
    public async Task<IActionResult> DeleteVariant(int variantId)
    {
        var result = await _productService.DeleteVariantAsync(variantId);
        if (result)
        {
            return NoContent();
        }

        return StatusCode(500, "Failed to delete variant");
    }
}