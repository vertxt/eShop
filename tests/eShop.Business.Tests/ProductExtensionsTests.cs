using eShop.Business.Extensions; // Adjust namespace as needed
using eShop.Data.Entities.ProductAggregate;
using eShop.Shared.Parameters;

namespace eShop.Business.Tests.Extensions
{
    public class ProductExtensionsTests
    {
        private IQueryable<Product> GetTestProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Gaming Laptop",
                    BasePrice = 1200.00m,
                    Description = "High-end gaming laptop",
                    ShortDescription = "Gaming laptop",
                    IsActive = true,
                    CategoryId = 1,
                    HasVariants = true,
                    QuantityInStock = 5,
                    CreatedDate = DateTime.UtcNow.AddDays(-30)
                },
                new Product
                {
                    Id = 2,
                    Name = "Office Laptop",
                    BasePrice = 800.00m,
                    Description = "Budget office laptop",
                    ShortDescription = "Office laptop",
                    IsActive = true,
                    CategoryId = 1,
                    HasVariants = false,
                    QuantityInStock = 15,
                    CreatedDate = DateTime.UtcNow.AddDays(-20)
                },
                new Product
                {
                    Id = 3,
                    Name = "Bluetooth Headphones",
                    BasePrice = 150.00m,
                    Description = "Wireless bluetooth headphones",
                    ShortDescription = "Wireless headphones",
                    IsActive = true,
                    CategoryId = 2,
                    HasVariants = true,
                    QuantityInStock = 40,
                    CreatedDate = DateTime.UtcNow.AddDays(-10)
                },
                new Product
                {
                    Id = 4,
                    Name = "Wired Mouse",
                    BasePrice = 25.00m,
                    Description = "Basic wired mouse",
                    ShortDescription = "Wired mouse",
                    IsActive = false,
                    CategoryId = 3,
                    HasVariants = false,
                    QuantityInStock = 0,
                    CreatedDate = DateTime.UtcNow.AddDays(-5)
                },
                new Product
                {
                    Id = 5,
                    Name = "Mechanical Keyboard",
                    BasePrice = 120.00m,
                    Description = "Gaming mechanical keyboard",
                    ShortDescription = "Mechanical keyboard",
                    IsActive = true,
                    CategoryId = 3,
                    HasVariants = true,
                    QuantityInStock = 60,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                }
            }.AsQueryable();

            return products;
        }

        [Fact]
        public void Search_NullOrEmptySearchTerm_ReturnsAllProducts()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var emptyResult = products.Search("");

            // Assert
            Assert.Equal(5, emptyResult.Count());
        }

        [Fact]
        public void Search_WithValidSearchTerm_ReturnsFilteredProducts()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var laptopResults = products.Search("laptop");
            var mouseResults = products.Search("mouse");

            // Assert
            Assert.Equal(2, laptopResults.Count());
            Assert.All(laptopResults, p => Assert.Contains("laptop", p.Name.ToLower()));
            
            Assert.Single(mouseResults);
            Assert.Contains("mouse", mouseResults.First().Name.ToLower());
        }

        [Fact]
        public void Search_WithCaseSensitiveSearchTerm_IgnoresCase()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var lowercaseResults = products.Search("keyboard");
            var uppercaseResults = products.Search("KEYBOARD");
            var mixedCaseResults = products.Search("KeyBoArd");

            // Assert
            Assert.Single(lowercaseResults);
            Assert.Single(uppercaseResults);
            Assert.Single(mixedCaseResults);
            
            Assert.Equal(lowercaseResults.First().Id, uppercaseResults.First().Id);
            Assert.Equal(lowercaseResults.First().Id, mixedCaseResults.First().Id);
        }

        [Fact]
        public void Sort_ByPrice_OrdersProductsByPrice()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var ascendingResults = products.Sort("price").ToList();
            var descendingResults = products.Sort("price-desc").ToList();

            // Assert
            Assert.Equal(5, ascendingResults.Count);
            Assert.Equal(5, descendingResults.Count);
            
            Assert.Equal(25.00m, ascendingResults.First().BasePrice);
            Assert.Equal(1200.00m, descendingResults.First().BasePrice);
        }

        [Fact]
        public void Sort_ByName_OrdersProductsByName()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var ascendingResults = products.Sort("name").ToList();
            var descendingResults = products.Sort("name-desc").ToList();

            // Assert
            Assert.Equal("Bluetooth Headphones", ascendingResults.First().Name);
            Assert.Equal("Wired Mouse", descendingResults.First().Name);
        }

        [Fact]
        public void Sort_ById_OrdersProductsById()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var ascendingResults = products.Sort("id").ToList();
            var descendingResults = products.Sort("id-desc").ToList();

            // Assert
            Assert.Equal(1, ascendingResults.First().Id);
            Assert.Equal(5, descendingResults.First().Id);
        }

        [Fact]
        public void Sort_ByCreatedDate_OrdersProductsByCreatedDate()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var ascendingResults = products.Sort("created-date").ToList();
            var descendingResults = products.Sort("created-date-desc").ToList();

            // Assert
            // The oldest product should be first in ascending order
            Assert.Equal(1, ascendingResults.First().Id); // Created 30 days ago
            // The newest product should be first in descending order
            Assert.Equal(5, descendingResults.First().Id); // Created 1 day ago
        }

        [Fact]
        public void Sort_WithInvalidSortParameter_ReturnsOriginalQuery()
        {
            // Arrange
            var products = GetTestProducts();
            var originalOrder = products.ToList();

            // Act
            var result = products.Sort("invalid-param").ToList();

            // Assert
            Assert.Equal(originalOrder.Count, result.Count);
            for (int i = 0; i < originalOrder.Count; i++)
            {
                Assert.Equal(originalOrder[i].Id, result[i].Id);
            }
        }

        [Fact]
        public void Filter_ByPriceRange_ReturnsProductsInRange()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                MinPrice = 100,
                MaxPrice = 500
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.InRange(p.BasePrice, 100, 500));
        }

        [Fact]
        public void Filter_ByCategoryIds_ReturnsProductsInCategories()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                CategoryIds = new List<int> { 1, 3 }
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.All(result, p => Assert.Contains(p.CategoryId, parameters.CategoryIds));
        }

        [Fact]
        public void Filter_ByOutOfStockRange_ReturnsOutOfStockProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                StockRange = StockRange.OutOfStock
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Single(result);
            Assert.All(result, p => Assert.True(p.QuantityInStock <= 0));
        }

        [Fact]
        public void Filter_ByLowStockRange_ReturnsLowStockProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                StockRange = StockRange.Low
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Single(result);
            Assert.All(result, p => Assert.InRange(p.QuantityInStock!.Value, 1, 10));
        }

        [Fact]
        public void Filter_ByMediumStockRange_ReturnsMediumStockProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                StockRange = StockRange.Medium
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.InRange(p.QuantityInStock!.Value, 11, 50));
        }

        [Fact]
        public void Filter_ByHighStockRange_ReturnsHighStockProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                StockRange = StockRange.High
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Single(result);
            Assert.All(result, p => Assert.True(p.QuantityInStock > 50));
        }

        [Fact]
        public void Filter_ByActiveStatus_ReturnsActiveProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                IsActive = true
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.All(result, p => Assert.True(p.IsActive));
        }

        [Fact]
        public void Filter_ByHasVariants_ReturnsProductsWithVariants()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                HasVariants = true
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, p => Assert.True(p.HasVariants));
        }

        [Fact]
        public void Filter_ByCreatedDateRange_ReturnsProductsInDateRange()
        {
            // Arrange
            var products = GetTestProducts();
            var today = DateTime.UtcNow;
            var parameters = new ProductParameters
            {
                CreatedAfter = today.AddDays(-15),
                CreatedBefore = today.AddDays(-2)
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Equal(2, result.Count); // Should include products created 10 and 5 days ago
        }

        [Fact]
        public void Filter_WithMultipleParameters_ReturnsCorrectlyFilteredProducts()
        {
            // Arrange
            var products = GetTestProducts();
            var parameters = new ProductParameters
            {
                MinPrice = 100,
                MaxPrice = 1000,
                CategoryIds = new List<int> { 1 },
                IsActive = true,
                HasVariants = false
            };

            // Act
            var result = products.Filter(parameters).ToList();

            // Assert
            Assert.Single(result);
            var product = result.First();
            Assert.Equal(2, product.Id); // Office Laptop
            Assert.InRange(product.BasePrice, parameters.MinPrice.Value, parameters.MaxPrice.Value);
            Assert.Equal(parameters.CategoryIds.First(), product.CategoryId);
            Assert.Equal(parameters.IsActive.Value, product.IsActive);
            Assert.Equal(parameters.HasVariants.Value, product.HasVariants);
        }

        [Fact]
        public void Filter_WithNullParameters_ReturnsAllProducts()
        {
            // Arrange
            var products = GetTestProducts();

            // Act
            var result = products.Filter(null).ToList();

            // Assert
            Assert.Equal(5, result.Count);
        }
    }
}