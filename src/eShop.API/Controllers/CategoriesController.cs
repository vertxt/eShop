using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Categories;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<CategoryDetailDto>> GetDetailsById(int id)
        {
            var category = await _categoryService.GetByIdWithDetailsAsync(id);
            return Ok(category);
        }

        [HttpGet("attributes/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<CategoryAttributeDto>>> GetAttributesByCategoryId(int categoryId)
        {
            var attributes = await _categoryService.GetAttributesByCategoryIdAsync(categoryId);
            return Ok(attributes);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto createCategoryDto)
        {
            var addedCategory = await _categoryService.CreateAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            _ = await _categoryService.UpdateAsync(id, updateCategoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}