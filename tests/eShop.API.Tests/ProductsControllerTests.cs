using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Abstractions;

namespace eShop.API.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductService> _mockProductService;
        private readonly ITestOutputHelper _output;

        public ProductsControllerTests(ITestOutputHelper output)
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
            _output = output;
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithPagedListOfProducts()
        {
            // Arrange
            var parameters = new ProductParameters();
            var products = new PagedList<ProductDto>(new List<ProductDto>(), 0, 1, 10);
            _mockProductService.Setup(service => service.GetAllAsync(parameters)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll(parameters);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(products, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            int id = 1;
            var product = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                BasePrice = 100
            };
            _mockProductService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetByUuid_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var uuid = Guid.NewGuid().ToString();
            var product = new ProductDto
            {
                Id = 1,
                Uuid = uuid,
                Name = "Test Product",
                BasePrice = 100,
            };
            _mockProductService.Setup(service => service.GetByUuidAsync(uuid)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetByUuid(uuid);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetDetailsById_ReturnsOkResult_WithProductDetails()
        {
            // Arrange
            int id = 1;
            var productDetails = new ProductDetailDto
            {
                Id = 1,
                Name = "Test Product",
                BasePrice = 100,
            };
            _mockProductService.Setup(service => service.GetDetailByIdAsync(id)).ReturnsAsync(productDetails);

            // Act
            var result = await _controller.GetDetailsById(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(productDetails, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithCreatedProduct()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                BasePrice = 100,
                Description = "Long Description",
                ShortDescription = "Short Description",
                CategoryId = 1,
                IsActive = true,
                HasVariants = false,
                QuantityInStock = 100,
            };
            var createdProduct = new ProductDto
            {
                Uuid = Guid.NewGuid().ToString(),
                Name = "New Product",
                BasePrice = 100,
                ShortDescription = "Thort Description",
                IsActive = true,
                HasVariants = false,
                QuantityInStock = 100,
            };
            _mockProductService.Setup(service => service.CreateAsync(createDto)).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equivalent(createdProduct, createdResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            int id = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
            };
            var updatedProduct = new ProductDto
            {
                Id = 1,
                Name = "Updated Name",
                BasePrice = 100,
            };
            _mockProductService.Setup(service => service.UpdateAsync(id, updateDto)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.Update(id, updateDto);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult?.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            int productId = 1;
            _mockProductService.Setup(service => service.DeleteAsync(productId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
    }
}
