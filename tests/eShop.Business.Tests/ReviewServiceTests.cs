using AutoMapper;
using eShop.Business.Services;
using eShop.Data.Entities.UserAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Reviews;
using Moq;

namespace eShop.Business.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfigurationProvider> _mockConfigProvider;
        private readonly ReviewService _service;

        public ReviewServiceTests()
        {
            _mockRepository = new Mock<IReviewRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockConfigProvider = new Mock<IConfigurationProvider>();

            // Setup mapper to provide configuration provider
            _mockMapper.Setup(m => m.ConfigurationProvider)
                .Returns(_mockConfigProvider.Object);

            _service = new ReviewService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddAsync_WithValidData_ShouldAddAndReturnReview()
        {
            // Arrange
            int productId = 1;
            string userId = "user1";
            var createDto = new CreateReviewDto
            {
                Rating = 5m,
                Title = "Great product",
                Body = "I love it"
            };

            var review = new Review
            {
                Rating = 5m,
                Title = "Great product",
                Body = "I love it"
            };

            var savedReview = new Review
            {
                Id = 1,
                ProductId = productId,
                UserId = userId,
                Rating = 5m,
                Title = "Great product",
                Body = "I love it",
                CreatedDate = DateTime.UtcNow
            };

            var reviewDto = new ReviewDto
            {
                Id = 1,
                ProductId = productId,
                Rating = 5m,
                Title = "Great product",
                Body = "I love it",
                Reviewer = "User 1",
                CreatedDate = savedReview.CreatedDate
            };

            _mockRepository.Setup(repo => repo.ExistsAsync(productId, userId))
                .ReturnsAsync(false);

            _mockMapper.Setup(mapper => mapper.Map<Review>(createDto))
                .Returns(review);

            _mockRepository.Setup(repo => repo.AddAsync(review))
                .Callback(() =>
                {
                    // Simulate DB saving with ID generation
                    review.Id = savedReview.Id;
                    review.ProductId = productId;
                    review.UserId = userId;
                    review.CreatedDate = savedReview.CreatedDate;
                })
                .ReturnsAsync(true);

            _mockMapper.Setup(mapper => mapper.Map<ReviewDto>(review))
                .Returns(reviewDto);

            // Act
            var result = await _service.AddAsync(productId, userId, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(productId, result.ProductId);
            Assert.Equal(5m, result.Rating);
            Assert.Equal("Great product", result.Title);
            Assert.Equal("I love it", result.Body);

            _mockRepository.Verify(repo => repo.ExistsAsync(productId, userId), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Review>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Review>(createDto), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<ReviewDto>(review), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WithNullUserId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            int productId = 1;
            string? userId = null;
            var createDto = new CreateReviewDto
            {
                Rating = 5m,
                Title = "Great product",
                Body = "I love it"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddAsync(productId, userId, createDto));

            Assert.Equal("Unauthenticated user cannot add product review.", exception.Message);

            _mockRepository.Verify(repo => repo.ExistsAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_WithExistingReview_ShouldThrowInvalidOperationException()
        {
            // Arrange
            int productId = 1;
            string userId = "user1";
            var createDto = new CreateReviewDto
            {
                Rating = 5m,
                Title = "Great product",
                Body = "I love it"
            };

            _mockRepository.Setup(repo => repo.ExistsAsync(productId, userId))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddAsync(productId, userId, createDto));

            Assert.Equal("User has already reviewed this product.", exception.Message);

            _mockRepository.Verify(repo => repo.ExistsAsync(productId, userId), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Review>()), Times.Never);
        }
    }
}