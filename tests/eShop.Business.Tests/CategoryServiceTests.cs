using AutoMapper;
using eShop.Business.Services;
using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using MockQueryable;
using Moq;

namespace eShop.Business.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CategoryService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            };

            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Electronics" },
                new CategoryDto { Id = 2, Name = "Clothing" }
            };

            var mockQueryable = categories.AsQueryable().BuildMock();

            _mockRepository.Setup(repo => repo.GetAll())
                .Returns(mockQueryable);

            _mockMapper.Setup(mapper => mapper.Map<List<CategoryDto>>(It.IsAny<List<Category>>()))
                .Returns(categoryDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<List<CategoryDto>>(It.IsAny<List<Category>>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Electronics" };
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Electronics" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);

            // Act
            var result = await _service.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Electronics", result.Name);
            _mockRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CategoryDto>(category), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int categoryId = 999;

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(categoryId));
            _mockRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_WithValidId_ShouldReturnCategoryWithDetails()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category
            {
                Id = categoryId,
                Name = "Electronics",
                Attributes = new List<CategoryAttribute>
                {
                    new CategoryAttribute { Id = 1, Name = "Color" }
                },
                Products = new List<Product>()
            };

            var categoryDetailDto = new CategoryDetailDto
            {
                Id = categoryId,
                Name = "Electronics",
                Attributes = new List<CategoryAttributeDto>
                {
                    new CategoryAttributeDto { Id = 1, Name = "Color" }
                },
                Products = new List<ProductDto>()
            };

            _mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(categoryId))
                .ReturnsAsync(category);

            _mockMapper.Setup(mapper => mapper.Map<CategoryDetailDto>(category))
                .Returns(categoryDetailDto);

            // Act
            var result = await _service.GetByIdWithDetailsAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Single(result.Attributes);
            _mockRepository.Verify(repo => repo.GetByIdWithDetailsAsync(categoryId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CategoryDetailDto>(category), Times.Once);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int categoryId = 999;

            _mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(categoryId))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdWithDetailsAsync(categoryId));
            _mockRepository.Verify(repo => repo.GetByIdWithDetailsAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetAttributesByCategoryIdAsync_ShouldReturnAttributes()
        {
            // Arrange
            int categoryId = 1;
            var attributes = new List<CategoryAttribute>
            {
                new CategoryAttribute { Id = 1, Name = "Color", CategoryId = categoryId },
                new CategoryAttribute { Id = 2, Name = "Size", CategoryId = categoryId }
            };

            var attributeDtos = new List<CategoryAttributeDto>
            {
                new CategoryAttributeDto { Id = 1, Name = "Color" },
                new CategoryAttributeDto { Id = 2, Name = "Size" }
            };

            var mockQueryable = attributes.AsQueryable().BuildMock();

            _mockRepository.Setup(repo => repo.GetAttributesByCategoryId(categoryId))
                .Returns(mockQueryable);

            _mockMapper.Setup(mapper => mapper.Map<List<CategoryAttributeDto>>(It.IsAny<List<CategoryAttribute>>()))
                .Returns(attributeDtos);

            // Act
            var result = await _service.GetAttributesByCategoryIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(repo => repo.GetAttributesByCategoryId(categoryId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<List<CategoryAttributeDto>>(It.IsAny<List<CategoryAttribute>>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndReturnCategory()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "New Category" };
            var category = new Category { Name = "New Category" };
            var savedCategory = new Category { Id = 1, Name = "New Category" };
            var categoryDto = new CategoryDto { Id = 1, Name = "New Category" };

            _mockMapper.Setup(mapper => mapper.Map<Category>(createDto))
                .Returns(category);

            _mockRepository.Setup(repo => repo.AddAsync(category))
                .Callback(() =>
                {
                    // Simulate DB saving with ID generation
                    category.Id = savedCategory.Id;
                })
                .ReturnsAsync(true);

            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Category", result.Name);
            _mockRepository.Verify(repo => repo.AddAsync(category), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Category>(createDto), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CategoryDto>(category), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateAndReturnCategory()
        {
            // Arrange
            int categoryId = 1;
            var updateDto = new UpdateCategoryDto { Name = "Updated Category", Description = "New description" };

            var existingCategory = new Category
            {
                Id = categoryId,
                Name = "Original Category",
                Description = "Original description",
                Attributes = new List<CategoryAttribute>
                {
                    new CategoryAttribute { Id = 1, Name = "Color" }
                }
            };

            var updatedCategory = new Category
            {
                Id = categoryId,
                Name = "Updated Category",
                Description = "New description",
                Attributes = new List<CategoryAttribute>()
            };

            var categoryDto = new CategoryDto
            {
                Id = categoryId,
                Name = "Updated Category",
                Description = "New description"
            };

            _mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockMapper.Setup(mapper => mapper.Map(updateDto, existingCategory))
                .Callback(() =>
                {
                    existingCategory.Name = updateDto.Name;
                    existingCategory.Description = updateDto.Description;
                });

            _mockRepository.Setup(repo => repo.UpdateAsync(existingCategory))
                .ReturnsAsync(true);

            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(existingCategory))
                .Returns(categoryDto);

            // Act
            var result = await _service.UpdateAsync(categoryId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Updated Category", result.Name);
            Assert.Equal("New description", result.Description);

            // Verify that Attributes.Clear() was called
            Assert.Empty(existingCategory.Attributes);

            _mockRepository.Verify(repo => repo.GetByIdWithDetailsAsync(categoryId), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAsync(existingCategory), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map(updateDto, existingCategory), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CategoryDto>(existingCategory), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int categoryId = 999;
            var updateDto = new UpdateCategoryDto { Name = "Updated Category" };

            _mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(categoryId))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(categoryId, updateDto));
            _mockRepository.Verify(repo => repo.GetByIdWithDetailsAsync(categoryId), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteCategory()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category to delete" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockRepository.Setup(repo => repo.DeleteAsync(categoryId))
                .ReturnsAsync(true);

            // Act
            await _service.DeleteAsync(categoryId);

            // Assert
            _mockRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int categoryId = 999;

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(categoryId));
            _mockRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}