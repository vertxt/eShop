using AutoMapper;
using CloudinaryDotNet.Actions;
using eShop.Business.Interfaces;
using eShop.Business.Services;
using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Entities.UserAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Products;
using Microsoft.AspNetCore.Http;
using MockQueryable;
using Moq;

namespace eShop.Business.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockImageService = new Mock<IImageService>();
            _mockMapper = new Mock<IMapper>();

            _productService = new ProductService(
                _mockProductRepository.Object,
                _mockCategoryRepository.Object,
                _mockImageService.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductExists_ReturnsProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", BasePrice = 10.99m };
            var expectedDto = new ProductDto { Id = productId, Name = "Test Product", BasePrice = 10.99m };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetByIdAsync(productId));
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_WhenProductExists_ReturnProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                BasePrice = 10.99m,
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Title = "Good Product 1",
                        Body = "Good Product Description 1",
                        Rating = 4,
                    },
                    new Review
                    {
                        Title = "Good Product 2",
                        Body = "Good Product Description 2",
                        Rating = 5,
                    },
                }
            };
            var expectedDto = new ProductDto { Id = productId, Name = "Test Product", BasePrice = 10.99m, AverageRating = 4.5m, ReviewCount = 2 };

            _mockProductRepository.Setup(repo => repo.GetByIdWithBasicDetailsAsync(productId))
                .ReturnsAsync(product);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.GetByIdWithBasicDetailsAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
            _mockProductRepository.Verify(repo => repo.GetByIdWithBasicDetailsAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetByUuidAsync_WhenProductExists_ReturnsProductDto()
        {
            // Arrange
            var uuid = "test-uuid";
            var product = new Product { Id = 1, Uuid = uuid, Name = "Test Product", Description = "Test Description", BasePrice = 10.99m };
            var expectedDto = new ProductDto { Id = 1, Uuid = uuid, Name = "Test Product", BasePrice = 10.99m };

            _mockProductRepository.Setup(repo => repo.GetByUuidAsync(uuid))
                .ReturnsAsync(product);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.GetByUuidAsync(uuid);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(uuid, result.Uuid);
            _mockProductRepository.Verify(repo => repo.GetByUuidAsync(uuid), Times.Once);
        }

        [Fact]
        public async Task GetByUuidAsync_WhenProductDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var uuid = "nonexistent-uuid";
            _mockProductRepository.Setup(repo => repo.GetByUuidAsync(uuid))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetByUuidAsync(uuid));
            _mockProductRepository.Verify(repo => repo.GetByUuidAsync(uuid), Times.Once);
        }

        [Fact]
        public async Task GetDetailByIdAsync_WhenProductExists_ReturnsProductDetailDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", BasePrice = 10.99m };
            var expectedDto = new ProductDetailDto { Id = productId, Name = "Test Product", BasePrice = 10.99m };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);

            _mockMapper.Setup(m => m.Map<ProductDetailDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.GetDetailByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            _mockProductRepository.Verify(repo => repo.GetByIdWithDetailsAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetDetailByIdAsync_WhenProductDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetDetailByIdAsync(productId));
            _mockProductRepository.Verify(repo => repo.GetByIdWithDetailsAsync(productId), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsCreatedProductDto()
        {
            // Arrange
            var categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 99.99m,
                CategoryId = categoryId,
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var category = new Category { Id = categoryId, Name = "Test Category" };
            var newProduct = new Product { Id = 1, Name = "New Product", CategoryId = categoryId, Description = "Test Description", BasePrice = 10.99m };
            var expectedDto = new ProductDto { Id = 1, Name = "New Product", CategoryName = "Test Category", BasePrice = 10.99m };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(newProduct);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithNonexistentCategory_ThrowsKeyNotFoundException()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 99.99m,
                CategoryId = 999
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.CreateAsync(createDto));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidAttribute_ThrowsInvalidOperationException()
        {
            // Arrange
            var categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 99.99m,
                CategoryId = categoryId,
                Attributes = new List<CreateProductAttributeDto>
                {
                    new CreateProductAttributeDto { AttributeId = 999, Value = "Invalid" }
                }
            };

            var category = new Category { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockCategoryRepository.Setup(repo => repo.GetAttributeByIdAsync(999))
                .ReturnsAsync((CategoryAttribute?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateAsync(createDto));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.GetAttributeByIdAsync(999), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_AttributeFromDifferentCategory_ThrowsInvalidOperationException()
        {
            // Arrnage
            int categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 10.99m,
                CategoryId = categoryId,
                Attributes = new List<CreateProductAttributeDto>
                {
                    new CreateProductAttributeDto { AttributeId = 1, Value = "From different category" }
                }
            };

            var category = new Category { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            var categoryAttributes = new List<CategoryAttribute>
            {
                new CategoryAttribute { Id = 1, CategoryId = categoryId, Name = "Category Attribute 1" },
                new CategoryAttribute { Id = 2, CategoryId = categoryId, Name = "Category Attribute 2" },
            }.AsQueryable().BuildMock();

            _mockCategoryRepository.Setup(repo => repo.GetAttributesByCategoryId(categoryId))
                .Returns(categoryAttributes);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateAsync(createDto));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.GetAttributeByIdAsync(1), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ImageMetadataMismatch_ThrowsInvalidOperationException()
        {
            // Arrange
            int categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 10.99m,
                CategoryId = categoryId,
                Images = new List<IFormFile> { Mock.Of<IFormFile>() },
                ImageMetadata = new List<ImageMetadataDto>(),
            };

            var newProduct = new Product
            {
                Id = 1,
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                CategoryId = categoryId,
                BasePrice = 10.99m,
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(new Category { Id = categoryId, Name = "Test Category" });
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateAsync(createDto));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }


        [Fact]
        public async Task CreateAsync_ImageUploadFails_ThrowsException()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            int categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 10.99m,
                CategoryId = categoryId,
                Images = new List<IFormFile> { Mock.Of<IFormFile>() },
                ImageMetadata = new List<ImageMetadataDto> { new ImageMetadataDto() }
            };

            var newProduct = new Product
            {
                Id = 1,
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                CategoryId = categoryId,
                BasePrice = 10.99m,
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category { Id = 1, Name = "Test Category" });

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            _mockImageService.Setup(service => service.AddImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new ImageUploadResult { Error = new Error { Message = "Upload failed" } });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _productService.CreateAsync(createDto));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_NoMainImage_SetsFirstImageAsMain()
        {
            // Arrange
            int categoryId = 1;
            var mockFile = new Mock<IFormFile>();
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 10.99m,
                CategoryId = categoryId,
                Images = new List<IFormFile> { mockFile.Object },
                ImageMetadata = new List<ImageMetadataDto> { new ImageMetadataDto { IsMain = false } }
            };

            var newProduct = new Product
            {
                Id = 1,
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                CategoryId = categoryId,
                BasePrice = 10.99m,
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category { Id = 1, Name = "Test Category" });

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            _mockImageService.Setup(service => service.AddImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new ImageUploadResult
                {
                    SecureUrl = new Uri("https://example.com/image.jpg"),
                    PublicId = "public_id"
                });

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(newProduct);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.CreateAsync(createDto);

            // Assert
            Assert.True(newProduct.Images.First().IsMain);
        }

        [Fact]
        public async Task CreateAsync_VariantWithoutSKU_GeneratesSKU()
        {
            // Arrange
            int categoryId = 1;
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 10.99m,
                CategoryId = categoryId,
                Variants = new List<CreateProductVariantDto>
                {
                    new CreateProductVariantDto { Name = "Variant 1", SKU = "" }
                }
            };

            var newProduct = new Product
            {
                Id = 1,
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                CategoryId = categoryId,
                BasePrice = 10.99m,
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Category { Id = 1, Name = "Test Category" });

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            _mockMapper.Setup(m => m.Map<ProductVariant>(It.IsAny<CreateProductVariantDto>()))
                .Returns(new ProductVariant { Name = "Variant 1", SKU = "" });

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(newProduct);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.CreateAsync(createDto);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(
                It.Is<Product>(p => p.Variants.Any(v => !string.IsNullOrEmpty(v.SKU)))),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithImagesAndMetadata_CreatesProductWithImages()
        {
            // Arrange
            var categoryId = 1;
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);

            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "Description",
                ShortDescription = "Short",
                BasePrice = 99.99m,
                CategoryId = categoryId,
                Images = new List<IFormFile> { mockFile.Object },
                ImageMetadata = new List<ImageMetadataDto> { new ImageMetadataDto { IsMain = true, DisplayOrder = 1 } }
            };

            var category = new Category { Id = categoryId, Name = "Test Category" };
            var newProduct = new Product
            {
                Id = 1,
                Name = "New Product",
                CategoryId = categoryId,
                Description = "New Description",
                BasePrice = 10.99m,
                Images = new List<ProductImage>()
            };
            var expectedDto = new ProductDto { Id = 1, Name = "New Product", CategoryName = "Test Category", BasePrice = 10.99m };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(newProduct);

            var imageUploadResult = new ImageUploadResult
            {
                SecureUrl = new Uri("https://example.com/image.jpg"),
                PublicId = "test_public_id"
            };

            _mockImageService.Setup(svc => svc.AddImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(imageUploadResult);

            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(newProduct);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _productService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            _mockImageService.Verify(svc => svc.AddImageAsync(It.IsAny<IFormFile>()), Times.Once);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ReturnsUpdatedProductDto()
        {
            // Arrange
            var productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Original Product",
                Description = "Original Description",
                BasePrice = 99.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            var updatedDto = new ProductDto
            {
                Id = productId,
                Name = "Updated Product",
                BasePrice = 149.99m
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Description = src.Description;
                    dest.BasePrice = src.BasePrice;
                });

            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(updatedDto);

            // Act
            var result = await _productService.UpdateAsync(productId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Product", result.Name);
            Assert.Equal(149.99m, result.BasePrice);
            _mockProductRepository.Verify(repo => repo.GetByIdWithDetailsAsync(productId), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonexistentProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                BasePrice = 149.99m,
                CategoryId = 1
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateAsync(productId, updateDto));
            _mockProductRepository.Verify(repo => repo.GetByIdWithDetailsAsync(productId), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_AddNewVariant_AddsVariantToProduct()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>
                {
                    new UpdateProductVariantDto { Name = "New Variant", Price = 50.00m }
                },
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            var newVariant = new ProductVariant { Name = "New Variant", Price = 50.00m };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(existingProduct);

            _mockMapper.Setup(m => m.Map<ProductVariant>(It.IsAny<UpdateProductVariantDto>()))
                .Returns(newVariant);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Contains(existingProduct.Variants, v => v.Name == "New Variant" && v.Price == 50.00m);
            Assert.True(existingProduct.HasVariants);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_RemoveExistingVariant_RemovesVariantFromProduct()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(), // No variants in update
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingVariant = new ProductVariant { Id = 1, Name = "Existing Variant" };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant> { existingVariant },
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(existingProduct);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(1, updateDto);

            // Assert
            Assert.DoesNotContain(existingVariant, existingProduct.Variants);
            Assert.False(existingProduct.HasVariants);
            Assert.Empty(existingProduct.Variants);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdateExistingVariant_UpdatesVariantProperties()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>
                {
                    new UpdateProductVariantDto
                    {
                        Id = 1,
                        Name = "Updated Variant",
                        Price = 75.00m
                    }
                },
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingVariant = new ProductVariant
            {
                Id = 1,
                Name = "Original Variant",
                Price = 50.00m
            };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant> { existingVariant },
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductVariantDto>(), It.IsAny<ProductVariant>()))
                .Callback<UpdateProductVariantDto, ProductVariant>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Price = src.Price;
                });

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(productId, updateDto);

            // Assert
            Assert.Equal("Updated Variant", existingVariant.Name);
            Assert.Equal(75.00m, existingVariant.Price);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ClearsExistingAttributes_AndAddsNewOnes()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>
                {
                    new CreateProductAttributeDto { AttributeId = 101, Value = "New Value 1" },
                    new CreateProductAttributeDto { AttributeId = 102, Value = "New Value 2" }
                },
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Id = 1, AttributeId = 201, Value = "Old Value 1" },
                    new ProductAttribute { Id = 2, AttributeId = 202, Value = "Old Value 2" }
                },
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(productId, updateDto);

            // Assert
            Assert.Equal(2, existingProduct.Attributes.Count);
            Assert.Contains(existingProduct.Attributes, a => a.AttributeId == 101 && a.Value == "New Value 1");
            Assert.Contains(existingProduct.Attributes, a => a.AttributeId == 102 && a.Value == "New Value 2");
            Assert.DoesNotContain(existingProduct.Attributes, a => a.AttributeId == 201);
            Assert.DoesNotContain(existingProduct.Attributes, a => a.AttributeId == 202);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithEmptyAttributes_ClearsAllExistingAttributes()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(), // Empty attributes
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Id = 1, AttributeId = 201, Value = "Old Value 1" },
                    new ProductAttribute { Id = 2, AttributeId = 202, Value = "Old Value 2" }
                },
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(productId, updateDto);

            // Assert
            Assert.Empty(existingProduct.Attributes);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_RemoveExistingImage_DeletesImageFromStorage()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(), // No existing images to keep
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingImage = new ProductImage
            {
                Id = 1,
                PublicId = "image_public_id",
                Url = "https://example.com/image.jpg"
            };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage> { existingImage }
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockImageService.Setup(service => service.DeleteImageAsync("image_public_id"))
                .ReturnsAsync(new DeletionResult());

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(productId, updateDto);

            // Assert
            _mockImageService.Verify(service => service.DeleteImageAsync("image_public_id"), Times.Once);
            _mockProductRepository.Verify(repo => repo.RemoveImageAsync(existingImage), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ImageDeletionFails_ThrowsInvalidOperationException()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(), // No existing images to keep
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingImage = new ProductImage
            {
                Id = 1,
                PublicId = "image_public_id",
                Url = "https://example.com/image.jpg"
            };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage> { existingImage }
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockImageService.Setup(service => service.DeleteImageAsync("image_public_id"))
                .ReturnsAsync(new DeletionResult { Error = new Error { Message = "Deletion failed" } });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _productService.UpdateAsync(1, updateDto));
        }

        [Fact]
        public async Task UpdateAsync_AddNewImage_UploadsAndAddsImageToProduct()
        {
            // Arrange
            int productId = 1;
            var mockFile = new Mock<IFormFile>();
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile> { mockFile.Object },
                ImageMetadata = new List<ImageMetadataDto> { new ImageMetadataDto { IsMain = true } }
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockImageService.Setup(service => service.AddImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new ImageUploadResult
                {
                    SecureUrl = new Uri("https://example.com/new-image.jpg"),
                    PublicId = "new_image_public_id"
                });

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(productId, updateDto);

            // Assert
            Assert.Contains(existingProduct.Images, i =>
                i.Url == "https://example.com/new-image.jpg" &&
                i.PublicId == "new_image_public_id" &&
                i.IsMain);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_ImageUploadFails_ThrowsException()
        {
            // Arrange
            int productId = 1;
            var mockFile = new Mock<IFormFile>();
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int>(),
                ExistingImageMetadata = new List<ImageMetadataDto>(),
                Images = new List<IFormFile> { mockFile.Object },
                ImageMetadata = new List<ImageMetadataDto> { new ImageMetadataDto() }
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage>()
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(existingProduct);

            _mockImageService.Setup(service => service.AddImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new ImageUploadResult { Error = new Error { Message = "Upload failed" } });

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.UpdateAsync(1, updateDto));
        }

        [Fact]
        public async Task UpdateAsync_UpdateExistingImageMetadata_UpdatesImageProperties()
        {
            // Arrange
            int productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<CreateProductAttributeDto>(),
                Variants = new List<UpdateProductVariantDto>(),
                ExistingImageIds = new List<int> { 1 },
                ExistingImageMetadata = new List<ImageMetadataDto>
                {
                    new ImageMetadataDto { IsMain = true, DisplayOrder = 2 }
                },
                Images = new List<IFormFile>(),
                ImageMetadata = new List<ImageMetadataDto>()
            };

            var existingImage = new ProductImage
            {
                Id = 1,
                Url = "https://example.com/image.jpg",
                IsMain = false,
                DisplayOrder = 1,
            };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                ShortDescription = "Updated Short",
                BasePrice = 149.99m,
                CategoryId = 1,
                Attributes = new List<ProductAttribute>(),
                Variants = new List<ProductVariant>(),
                Images = new List<ProductImage> { existingImage }
            };

            _mockProductRepository.Setup(repo => repo.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(existingProduct);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback<UpdateProductDto, Product>((src, dest) => { });

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto());

            // Act
            await _productService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Equal("https://example.com/image.jpg", existingImage.Url);
            Assert.True(existingImage.IsMain);
            Assert.Equal(2, existingImage.DisplayOrder);
            Assert.Contains(existingProduct.Images, i =>
                i.Url == "https://example.com/image.jpg" &&
                i.DisplayOrder == 2 &&
                i.IsMain);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingProduct_DeletesProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Id = 1,
                        Url = "https://example.com/image.jpg",
                        PublicId = "test_public_id"
                    }
                },
                BasePrice = 10.99m,
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockImageService.Setup(svc => svc.DeleteImageAsync(It.IsAny<string>()))
                .ReturnsAsync(new DeletionResult());

            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(productId))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _mockImageService.Verify(svc => svc.DeleteImageAsync("test_public_id"), Times.Once);
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonexistentProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.DeleteAsync(productId));
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(productId), Times.Never);
        }


        [Fact]
        public async Task DeleteAsync_WithImages_DeletesImagesFromStorage()
        {
            // Arrange
            int productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Images = new List<ProductImage>
                {
                    new ProductImage { Id = 1, PublicId = "image1_public_id", Url = "https://example.com/image1.jpg" },
                    new ProductImage { Id = 2, PublicId = "image2_public_id", Url = "https://example.com/image2.jpg" }
                },
                BasePrice = 10.99m,
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockImageService.Setup(service => service.DeleteImageAsync(It.IsAny<string>()))
                .ReturnsAsync(new DeletionResult());

            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockImageService.Verify(service => service.DeleteImageAsync("image1_public_id"), Times.Once);
            _mockImageService.Verify(service => service.DeleteImageAsync("image2_public_id"), Times.Once);
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ImageDeletionFails_ThrowsInvalidOperationException()
        {
            // Arrange
            int productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Images = new List<ProductImage>
                {
                    new ProductImage { Id = 1, PublicId = "image_public_id", Url = "https://example.com/image.jpg" }
                },
                BasePrice = 10.99m,
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            _mockImageService.Setup(service => service.DeleteImageAsync("image_public_id"))
                .ReturnsAsync(new DeletionResult { Error = new Error { Message = "Deletion failed" } });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.DeleteAsync(1));
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(productId), Times.Never);
        }
    }
}