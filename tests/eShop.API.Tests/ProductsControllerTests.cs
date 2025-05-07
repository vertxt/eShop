using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Abstractions;

namespace eShop.API.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;
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
            var parameters = new ProductParameters { PageNumber = 1, PageSize = 10 };
            var products = new PagedList<ProductDto>(
                new List<ProductDto>
                {
                    new ProductDto { Id = 1, Name = "Product 1", BasePrice = 99.99m },
                    new ProductDto { Id = 2, Name = "Product 2", BasePrice = 149.99m }
                },
                2, 1, 10);

            _mockProductService.Setup(service => service.GetAllAsync(parameters)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll(parameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PagedList<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Items.Count());
            Assert.Equal(2, returnValue.Metadata.TotalCount);
            _mockProductService.Verify(s => s.GetAllAsync(parameters), Times.Once);
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
                BasePrice = 99.99m
            };
            _mockProductService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
            Assert.Equal("Test Product", returnValue.Name);
            Assert.Equal(99.99m, returnValue.BasePrice);
            _mockProductService.Verify(s => s.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByUuid_ReturnsOkResult_WithProduct()
        {
            // Arrange
            string uuid = "12345678-1234-1234-1234-123456789012";
            var expectedProduct = new ProductDto
            {
                Id = 1,
                Uuid = uuid,
                Name = "Test Product",
                BasePrice = 99.99m
            };

            _mockProductService.Setup(s => s.GetByUuidAsync(uuid))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetByUuid(uuid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(uuid, returnValue.Uuid);
            Assert.Equal("Test Product", returnValue.Name);
            _mockProductService.Verify(s => s.GetByUuidAsync(uuid), Times.Once);
        }

        [Fact]
        public async Task GetDetailsById_ReturnsOkResult_WithProductDetails()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new ProductDetailDto
            {
                Id = productId,
                Name = "Test Product",
                BasePrice = 99.99m,
                Description = "Detailed description",
                Images = new List<ProductImageDto>
                {
                    new ProductImageDto { Id = 1, Url = "image1.jpg" }
                }
            };

            _mockProductService.Setup(s => s.GetDetailByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetDetailsById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductDetailDto>(okResult.Value);
            Assert.Equal(productId, returnValue.Id);
            Assert.Equal("Test Product", returnValue.Name);
            Assert.Equal("Detailed description", returnValue.Description);
            Assert.Single(returnValue.Images);
            _mockProductService.Verify(s => s.GetDetailByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithNewProduct()
        {
            // Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "New Product",
                BasePrice = 149.99m,
                Description = "Product description",
                CategoryId = 1
            };

            var createdProduct = new ProductDto
            {
                Id = 3,
                Name = "New Product",
                BasePrice = 149.99m
            };

            _mockProductService.Setup(s => s.CreateAsync(createProductDto))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(createProductDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(3, createdAtActionResult.RouteValues["id"]);
            var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
            Assert.Equal(3, returnValue.Id);
            Assert.Equal("New Product", returnValue.Name);
            _mockProductService.Verify(s => s.CreateAsync(createProductDto), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            int productId = 1;
            var updateProductDto = new UpdateProductDto
            {
                Name = "Updated Product",
                BasePrice = 199.99m,
                Description = "Updated description",
                CategoryId = 2
            };

            // Act
            var result = await _controller.Update(productId, updateProductDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockProductService.Verify(s => s.UpdateAsync(productId, updateProductDto), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockProductService.Verify(s => s.DeleteAsync(productId), Times.Once);
        }
    }
}
