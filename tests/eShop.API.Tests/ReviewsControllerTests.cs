using System.Security.Claims;
using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;
using eShop.Shared.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eShop.API.Tests.Controllers
{
    public class ReviewsControllerTests
    {
        private readonly Mock<IReviewService> _mockReviewService;
        private readonly ReviewsController _controller;
        private readonly string _userId = "test-user-id";

        public ReviewsControllerTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _controller = new ReviewsController(_mockReviewService.Object);
            
            // Setup controller context with authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId)
            }, "test"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithPagedReviews()
        {
            // Arrange
            int productId = 1;
            var paginationParameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };
            
            var expectedReviews = new PagedList<ReviewDto>(
                new List<ReviewDto>
                {
                    new ReviewDto
                    { 
                        Id = 1, 
                        Rating = 5, 
                        Title = "Great product!", 
                        Reviewer = "User1" 
                    },
                    new ReviewDto
                    { 
                        Id = 2, 
                        Rating = 4, 
                        Title = "Good product", 
                        Reviewer = "User2" 
                    }
                },
                2, 1, 10);

            _mockReviewService.Setup(s => s.GetProductReviewsAsync(productId, paginationParameters))
                .ReturnsAsync(expectedReviews);

            // Act
            var result = await _controller.Get(productId, paginationParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PagedList<ReviewDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Items.Count());
            Assert.Equal(2, returnValue.Metadata.TotalCount);
            _mockReviewService.Verify(s => s.GetProductReviewsAsync(productId, paginationParameters), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WithNewReview()
        {
            // Arrange
            int productId = 1;
            var createReviewDto = new CreateReviewDto
            {
                Rating = 5,
                Title = "Excellent product, highly recommended!"
            };

            var createdReview = new ReviewDto
            {
                Id = 3,
                ProductId = productId,
                Reviewer = "TestUser",
                Rating = 5,
                Title = "Excellent product, highly recommended!"
            };

            _mockReviewService.Setup(s => s.AddAsync(productId, _userId, createReviewDto))
                .ReturnsAsync(createdReview);

            // Act
            var result = await _controller.Post(productId, createReviewDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(productId, createdAtActionResult.RouteValues["productId"]);
            var returnValue = Assert.IsType<ReviewDto>(createdAtActionResult.Value);
            Assert.Equal(3, returnValue.Id);
            Assert.Equal(5, returnValue.Rating);
            Assert.Equal("Excellent product, highly recommended!", returnValue.Title);
            _mockReviewService.Verify(s => s.AddAsync(productId, _userId, createReviewDto), Times.Once);
        }

        [Fact]
        public async Task Post_UsesUserIdFromClaims_ToAddReview()
        {
            // Arrange
            int productId = 1;
            var createReviewDto = new CreateReviewDto
            {
                Rating = 4,
                Title = "Good product"
            };

            var createdReview = new ReviewDto
            {
                Id = 4,
                ProductId = productId,
                Reviewer = "TestUser",
                Rating = 4,
                Title = "Good product",
            };

            _mockReviewService.Setup(s => s.AddAsync(productId, _userId, createReviewDto))
                .ReturnsAsync(createdReview);

            // Act
            var result = await _controller.Post(productId, createReviewDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<ReviewDto>(createdAtActionResult.Value);
            Assert.Equal("TestUser", returnValue.Reviewer);
            _mockReviewService.Verify(s => s.AddAsync(productId, _userId, createReviewDto), Times.Once);
        }
    }
}