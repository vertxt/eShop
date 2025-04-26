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

        public async Task<CategoryDetailDto> GetDetailByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryWithDetailsByIdAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID ${id} not found");
            }

            return _mapper.Map<CategoryDetailDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await _categoryRepository.AddAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory is null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

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

        public async Task<int> AddAttributeAsync(int categoryId, CreateCategoryAttributeDto createCategoryAttributeDto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");
            }

            var attribute = _mapper.Map<CategoryAttribute>(createCategoryAttributeDto);
            attribute.CategoryId = categoryId;

            await _categoryRepository.AddAttributeAsync(attribute);
            return attribute.Id;
        }

        public async Task<bool> UpdateAttributeAsync(int attributeId, UpdateCategoryAttributeDto updateCategoryAttributeDto)
        {
            var attribute = await _categoryRepository.GetAttributeByIdAsync(attributeId);
            if (attribute is null)
            {
                throw new KeyNotFoundException($"Category attribute with ID {attributeId} not found");
            }

            _mapper.Map(updateCategoryAttributeDto, attribute);

            return await _categoryRepository.UpdateAttributeAsync(attribute);
        }

        public async Task<bool> DeleteAttributeAsync(int attributeId)
        {
            var attribute = _categoryRepository.GetAttributeByIdAsync(attributeId);
            if (attribute is null)
            {
                throw new KeyNotFoundException($"Category Attribute with ID {attributeId} not found");
            }
            return await _categoryRepository.DeleteAttributeAsync(attributeId);
        }
    }
}