using eShop.Business.Services;
using eShop.Data.Entities.Products;
using eShop.Data.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using Xunit.Abstractions;

namespace eShop.Business.Tests
{
    public class ProductServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;

        public ProductServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
        }

        [Fact]
        public async Task Test_GetAllProductsAsync()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Uuid = "1", Name = "Product1", Description = "Description 1", BasePrice = 9.55m },
                new Product { Uuid = "2", Name = "Product2", Description = "Description 2", BasePrice = 4.55m }
            };

            var mock = mockProducts.BuildMock();

            _mockRepository.Setup(repository => repository.GetAll()).Returns(mock);

            var productService = new ProductService(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Product1");
            Assert.Contains(result, p => p.Name == "Product2");
        }
    }
}