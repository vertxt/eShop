using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eShop.API.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var expectedCategories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Electronics" },
                new CategoryDto { Id = 2, Name = "Clothing" }
            };

            _mockCategoryService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
            _mockCategoryService.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithCategory()
        {
            // Arrange
            int categoryId = 1;
            var expectedCategory = new CategoryDto 
            { 
                Id = categoryId, 
                Name = "Electronics",
                Description = "Electronic devices and accessories"
            };

            _mockCategoryService.Setup(s => s.GetByIdAsync(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _controller.GetById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal(categoryId, returnValue.Id);
            Assert.Equal("Electronics", returnValue.Name);
            _mockCategoryService.Verify(s => s.GetByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetDetailsById_ReturnsOkResult_WithCategoryDetails()
        {
            // Arrange
            int categoryId = 1;
            var expectedCategory = new CategoryDetailDto 
            { 
                Id = categoryId, 
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                Products = new List<ProductDto> 
                {
                    new ProductDto { Id = 1, Name = "Laptop", BasePrice = 999.99m },
                    new ProductDto { Id = 2, Name = "Smartphone", BasePrice = 499.99m }
                }
            };

            _mockCategoryService.Setup(s => s.GetByIdWithDetailsAsync(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _controller.GetDetailsById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CategoryDetailDto>(okResult.Value);
            Assert.Equal(categoryId, returnValue.Id);
            Assert.Equal("Electronics", returnValue.Name);
            Assert.Equal(2, returnValue.Products.Count());
            _mockCategoryService.Verify(s => s.GetByIdWithDetailsAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetAttributesByCategoryId_ReturnsOkResult_WithAttributes()
        {
            // Arrange
            int categoryId = 1;
            var expectedAttributes = new List<CategoryAttributeDto>
            {
                new CategoryAttributeDto { Id = 1, Name = "Brand" },
                new CategoryAttributeDto { Id = 2, Name = "Color" }
            };

            _mockCategoryService.Setup(s => s.GetAttributesByCategoryIdAsync(categoryId))
                .ReturnsAsync(expectedAttributes);

            // Act
            var result = await _controller.GetAttributesByCategoryId(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryAttributeDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
            _mockCategoryService.Verify(s => s.GetAttributesByCategoryIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithNewCategory()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto
            {
                Name = "New Category",
                Description = "New category description"
            };

            var createdCategory = new CategoryDto
            {
                Id = 3,
                Name = "New Category",
                Description = "New category description"
            };

            _mockCategoryService.Setup(s => s.CreateAsync(createCategoryDto))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.Create(createCategoryDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(3, createdAtActionResult.RouteValues["id"]);
            var returnValue = Assert.IsType<CategoryDto>(createdAtActionResult.Value);
            Assert.Equal(3, returnValue.Id);
            Assert.Equal("New Category", returnValue.Name);
            _mockCategoryService.Verify(s => s.CreateAsync(createCategoryDto), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            int categoryId = 1;
            var updateCategoryDto = new UpdateCategoryDto
            {
                Name = "Updated Category",
                Description = "Updated description"
            };

            // Act
            var result = await _controller.Update(categoryId, updateCategoryDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockCategoryService.Verify(s => s.UpdateAsync(categoryId, updateCategoryDto), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            int categoryId = 1;

            // Act
            var result = await _controller.Delete(categoryId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockCategoryService.Verify(s => s.DeleteAsync(categoryId), Times.Once);
        }
    }
}