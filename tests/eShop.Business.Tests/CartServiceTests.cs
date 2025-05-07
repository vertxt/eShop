using AutoMapper;
using eShop.Business.Services;
using eShop.Data.Entities.CartAggregate;
using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Carts;
using Moq;

namespace eShop.Business.Tests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _cartService = new CartService(_mockMapper.Object, _mockCartRepository.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnMappedCartDto()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            var cart = new Cart { Id = 1, UserId = userId, SessionId = sessionId };
            var cartDto = new CartDto { Id = 1 };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.GetCartAsync(userId, sessionId);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.GetCartAsync(userId, sessionId), Times.Once);
            _mockMapper.Verify(m => m.Map<CartDto>(cart), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_WithExistingItem_ShouldUpdateQuantity()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int productId = 1;
            int? variantId = null;
            int cartId = 1;
            int originalQuantity = 1;
            int quantity = 2;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                BasePrice = 10.99m,
                IsActive = true,
                QuantityInStock = 10,
                Variants = new List<ProductVariant>()
            };
            var existingItem = new CartItem
            {
                Id = 1,
                CartId = cartId,
                ProductId = productId,
                Quantity = originalQuantity
            };
            var cartDto = new CartDto { Id = cartId };
            var addToCartDto = new AddToCartDto { ProductId = productId, VariantId = variantId, Quantity = quantity };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);
            _mockCartRepository.Setup(r => r.GetCartItemAsync(cartId, productId, variantId))
                .ReturnsAsync(existingItem);
            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.AddToCartAsync(userId, sessionId, addToCartDto);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.UpdateCartItemAsync(It.Is<CartItem>(i =>
                i.Id == existingItem.Id &&
                i.Quantity == originalQuantity + quantity)), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_WithNewItem_ShouldAddNewCartItem()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int productId = 1;
            int? variantId = null;
            int cartId = 1;
            int quantity = 2;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                BasePrice = 10.99m,
                IsActive = true,
                QuantityInStock = 10,
                Variants = new List<ProductVariant>()
            };
            var cartDto = new CartDto { Id = cartId };
            var addToCartDto = new AddToCartDto { ProductId = productId, VariantId = variantId, Quantity = quantity };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);
            _mockCartRepository.Setup(r => r.GetCartItemAsync(cartId, productId, variantId))
                .ReturnsAsync((CartItem?)null);
            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.AddToCartAsync(userId, sessionId, addToCartDto);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.AddCartItemAsync(It.Is<CartItem>(i =>
                i.CartId == cartId &&
                i.ProductId == productId &&
                i.ProductVariantId == variantId &&
                i.Quantity == quantity)), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_WithInactiveProduct_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int productId = 1;

            var cart = new Cart { Id = 1, UserId = userId, SessionId = sessionId };
            var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", BasePrice = 10.99m, IsActive = false };
            var addToCartDto = new AddToCartDto { ProductId = productId, Quantity = 1 };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _cartService.AddToCartAsync(userId, sessionId, addToCartDto));
        }

        [Fact]
        public async Task AddToCartAsync_WithInsufficientStock_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int productId = 1;

            var cart = new Cart { Id = 1, UserId = userId, SessionId = sessionId };
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                BasePrice = 10.99m,
                IsActive = true,
                QuantityInStock = 2,
                Variants = new List<ProductVariant>()
            };
            var addToCartDto = new AddToCartDto { ProductId = productId, Quantity = 3 };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _cartService.AddToCartAsync(userId, sessionId, addToCartDto));
        }

        [Fact]
        public async Task AddToCartAsync_WithVariant_InsufficientStock_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int productId = 1;
            int variantId = 1;

            var cart = new Cart { Id = 1, UserId = userId, SessionId = sessionId };
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                BasePrice = 10.99m,
                IsActive = true,
                Variants = new List<ProductVariant> {
                    new ProductVariant { Id = variantId, Name = "Test Variant", IsActive = true, QuantityInStock = 2 }
                }
            };
            var addToCartDto = new AddToCartDto { ProductId = productId, VariantId = variantId, Quantity = 3 };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(productId))
                .ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _cartService.AddToCartAsync(userId, sessionId, addToCartDto));
        }

        [Fact]
        public async Task UpdateCartItemAsync_ReduceQuantity_ShouldUpdateItem()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int cartId = 1;
            int itemId = 2;
            int newQuantity = 2;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var cartItem = new CartItem
            {
                Id = itemId,
                CartId = cartId,
                Product = new Product { Name = "Test Product", Description = "Test Description", BasePrice = 10.99m, QuantityInStock = 10 },
                Quantity = 3
            };
            var updateDto = new UpdateCartItemDto { Id = itemId, Quantity = newQuantity };
            var cartDto = new CartDto { Id = cartId };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.GetCartItemAsync(itemId))
                .ReturnsAsync(cartItem);
            _mockCartRepository.Setup(r => r.GetCartByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.UpdateCartItemAsync(userId, sessionId, updateDto);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.UpdateCartItemAsync(It.Is<CartItem>(i =>
                i.Id == itemId && i.Quantity == newQuantity)), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItemAsync_SetQuantityToZero_ShouldRemoveItem()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int cartId = 1;
            int itemId = 2;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var cartItem = new CartItem { Id = itemId, CartId = cartId };
            var updateDto = new UpdateCartItemDto { Id = itemId, Quantity = 0 };
            var cartDto = new CartDto { Id = cartId };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.GetCartItemAsync(itemId))
                .ReturnsAsync(cartItem);
            _mockCartRepository.Setup(r => r.GetCartByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.UpdateCartItemAsync(userId, sessionId, updateDto);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.RemoveCartItemsAsync(cartItem), Times.Once);
        }

        [Fact]
        public async Task RemoveCartItemAsync_ShouldRemoveItemAndReturnUpdatedCart()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int cartId = 1;
            int itemId = 2;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var cartItem = new CartItem { Id = itemId, CartId = cartId };
            var cartDto = new CartDto { Id = cartId };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.GetCartItemAsync(itemId))
                .ReturnsAsync(cartItem);
            _mockCartRepository.Setup(r => r.GetCartByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(cartDto);

            // Act
            var result = await _cartService.RemoveCartItemAsync(userId, sessionId, itemId);

            // Assert
            Assert.Equal(cartDto, result);
            _mockCartRepository.Verify(r => r.RemoveCartItemsAsync(cartItem), Times.Once);
        }

        [Fact]
        public async Task ClearCartAsync_ShouldClearCartAndReturnEmptyCart()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            int cartId = 1;

            var cart = new Cart { Id = cartId, UserId = userId, SessionId = sessionId };
            var emptyCartDto = new CartDto { Id = cartId };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart))
                .Returns(emptyCartDto);

            // Act
            var result = await _cartService.ClearCartAsync(userId, sessionId);

            // Assert
            Assert.Equal(emptyCartDto, result);
            _mockCartRepository.Verify(r => r.ClearCartAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task GetCartSummaryAsync_ShouldReturnCartSummary()
        {
            // Arrange
            string userId = "user123";
            string sessionId = "session456";
            var cart = new Cart { Id = 1, UserId = userId, SessionId = sessionId };
            var summaryDto = new CartSummaryDto { TotalItems = 3, TotalPrice = 150.00m };

            _mockCartRepository.Setup(r => r.GetCartAsync(userId, sessionId))
                .ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartSummaryDto>(cart))
                .Returns(summaryDto);

            // Act
            var result = await _cartService.GetCartSummaryAsync(userId, sessionId);

            // Assert
            Assert.Equal(summaryDto, result);
            _mockCartRepository.Verify(r => r.GetCartAsync(userId, sessionId), Times.Once);
            _mockMapper.Verify(m => m.Map<CartSummaryDto>(cart), Times.Once);
        }
    }
}