using AutoMapper;
using eShop.API.Controller;
using eShop.Business.Interfaces;
using eShop.Data.Entities.ProductAggregate;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace eShop.API.Tests
{
    public class ProductsControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;

        public ProductsControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Test_GetAllProductsAsync()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Uuid = "1", Name = "Product1", Description = "Description 1", BasePrice = 10.99m },
                new Product { Uuid = "2", Name = "Product2", Description = "Description 2", BasePrice = 8.49m }
            };

            var mockProductDtos = new List<ProductDto>
            {
                new ProductDto { Name = "Product1", Description = "Description 1", BasePrice = 10.99m },
                new ProductDto { Name = "Product2", Description = "Description 2", BasePrice = 8.49m }
            };

            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(mockProducts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                       .Returns(mockProductDtos);

            var controller = new ProductsController(_mockProductService.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await controller.GetProductsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _testOutputHelper.WriteLine($"Returned value: {System.Text.Json.JsonSerializer.Serialize(okResult.Value)}");
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Collection(returnValue,
                item => Assert.Equal("Product1", item.Name),
                item => Assert.Equal("Product2", item.Name)
            );
        }
    }
}
