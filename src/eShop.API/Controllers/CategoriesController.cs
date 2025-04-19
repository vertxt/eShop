using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}