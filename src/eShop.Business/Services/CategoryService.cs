using AutoMapper;
using eShop.Business.Interfaces;
using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Categories;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID ${id} not found");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDetailDto> GetByIdWithDetailsAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithDetailsAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID ${id} not found");
            }

            return _mapper.Map<CategoryDetailDto>(category);
        }

        public async Task<IEnumerable<CategoryAttributeDto>> GetAttributesByCategoryIdAsync(int categoryId)
        {
            var attributes = await _categoryRepository.GetAttributesByCategoryId(categoryId).ToListAsync();
            return _mapper.Map<List<CategoryAttributeDto>>(attributes);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await _categoryRepository.AddAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdWithDetailsAsync(id);

            if (existingCategory is null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            existingCategory.Attributes.Clear();
            _mapper.Map(updateCategoryDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);

            return _mapper.Map<CategoryDto>(existingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            await _categoryRepository.DeleteAsync(id);
        }
    }
}