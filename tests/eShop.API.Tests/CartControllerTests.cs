using System.Security.Claims;
using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eShop.API.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly CartController _controller;
        private readonly string _userId = "test-user-id";
        private readonly string _sessionId = "test-session-id";

        public CartControllerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartController(_mockCartService.Object);

            // Setup controller context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId)
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Setup cookies
            _controller.ControllerContext.HttpContext.Request.Headers["Cookie"] = $"CartSessionId={_sessionId}";
        }

        [Fact]
        public async Task GetCart_ReturnsOkResult_WithCart()
        {
            // Arrange
            var expectedCart = new CartDto { };
            _mockCartService.Setup(s => s.GetCartAsync(_userId, _sessionId))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.GetCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(expectedCart, returnValue);
            _mockCartService.Verify(s => s.GetCartAsync(_userId, _sessionId), Times.Once);
        }

        [Fact]
        public async Task GetCartSummary_ReturnsOkResult_WithCartSummary()
        {
            // Arrange
            var expectedSummary = new CartSummaryDto
            {
                TotalItems = 2,
                TotalPrice = 149.99m
            };

            _mockCartService.Setup(s => s.GetCartSummaryAsync(_userId, _sessionId))
                .ReturnsAsync(expectedSummary);

            // Act
            var result = await _controller.GetCartSummary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartSummaryDto>(okResult.Value);
            Assert.Equal(expectedSummary.TotalItems, returnValue.TotalItems);
            Assert.Equal(expectedSummary.TotalPrice, returnValue.TotalPrice);
            _mockCartService.Verify(s => s.GetCartSummaryAsync(_userId, _sessionId), Times.Once);
        }

        [Fact]
        public async Task AddToCart_ReturnsOkResult_WithUpdatedCart()
        {
            // Arrange
            var addToCartDto = new AddToCartDto
            {
                ProductId = 1,
                Quantity = 2
            };

            var expectedCart = new CartDto { };

            _mockCartService.Setup(s => s.AddToCartAsync(_userId, _sessionId, addToCartDto))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.AddToCart(addToCartDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(expectedCart, returnValue);
            _mockCartService.Verify(s => s.AddToCartAsync(_userId, _sessionId, addToCartDto), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItem_ReturnsOkResult_WithUpdatedCart()
        {
            // Arrange
            var updateCartDto = new UpdateCartItemDto
            {
                Id = 1,
                Quantity = 3
            };

            var expectedCart = new CartDto { /* initialize properties */ };

            _mockCartService.Setup(s => s.UpdateCartItemAsync(_userId, _sessionId, updateCartDto))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.UpdateCartItem(updateCartDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(expectedCart, returnValue);
            _mockCartService.Verify(s => s.UpdateCartItemAsync(_userId, _sessionId, updateCartDto), Times.Once);
        }

        [Fact]
        public async Task RemoveCartItem_ReturnsOkResult_WithUpdatedCart()
        {
            // Arrange
            int cartItemId = 1;
            var expectedCart = new CartDto { };

            _mockCartService.Setup(s => s.RemoveCartItemAsync(_userId, _sessionId, cartItemId))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.RemoveCartItem(cartItemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(expectedCart, returnValue);
            _mockCartService.Verify(s => s.RemoveCartItemAsync(_userId, _sessionId, cartItemId), Times.Once);
        }

        [Fact]
        public async Task ClearCart_ReturnsOkResult_WithEmptyCart()
        {
            // Arrange
            var expectedCart = new CartDto { };

            _mockCartService.Setup(s => s.ClearCartAsync(_userId, _sessionId))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.ClearCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(expectedCart, returnValue);
            _mockCartService.Verify(s => s.ClearCartAsync(_userId, _sessionId), Times.Once);
        }

        [Fact]
        public async Task GetOrCreateSessionId_CreatesCookie_WhenNoCookieExists()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Request.Headers.Remove("Cookie");

            // Act
            var privateMethod = typeof(CartController).GetMethod("GetOrCreateSessionId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var sessionId = privateMethod.Invoke(_controller, null) as string;

            // Assert
            Assert.NotNull(sessionId);
            Assert.NotEmpty(sessionId);

            // Verify cookie was set
            var cookies = _controller.ControllerContext.HttpContext.Response.Headers["Set-Cookie"];
            Assert.Contains("CartSessionId", cookies.ToString());
        }
    }
}