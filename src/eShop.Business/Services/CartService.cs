using AutoMapper;
using eShop.Business.Interfaces;
using eShop.Data.Entities.CartAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Carts;

namespace eShop.Business.Services
{
    public class CartService : ICartService
    {
        IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IMapper mapper, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartDto> GetCartAsync(string userId, string sessionId)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> AddToCartAsync(string userId, string sessionId, AddToCartDto addToCartDto)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);

            var product = await _productRepository.GetByIdWithDetailsAsync(addToCartDto.ProductId);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {addToCartDto.ProductId} not found");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException("Product is not available");
            }

            // Check stock availability
            if (addToCartDto.VariantId.HasValue)
            {
                var variant = product.Variants.FirstOrDefault(v => v.Id == addToCartDto.VariantId);
                if (variant == null)
                    throw new KeyNotFoundException($"Product variant with ID {addToCartDto.VariantId} not found");

                if (!variant.IsActive)
                    throw new InvalidOperationException("Product variant is not available");

                if (variant.QuantityInStock < addToCartDto.Quantity)
                    throw new InvalidOperationException("Not enough stock available");
            }
            else
            {
                if (product.QuantityInStock < addToCartDto.Quantity)
                    throw new InvalidOperationException("Not enough stock available");
            }

            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, product.Id, addToCartDto.VariantId);
            if (existingItem != null)
            {
                existingItem.Quantity += addToCartDto.Quantity;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = cart.Id,
                    ProductVariantId = addToCartDto.VariantId,
                    Quantity = addToCartDto.Quantity,
                };
                await _cartRepository.AddCartItemAsync(newCartItem);
            }

            cart = await _cartRepository.GetByIdAsync(cart.Id);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> UpdateCartItemAsync(string userId, string sessionId, UpdateCartItemDto updateCartDto)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);
            var item = await _cartRepository.GetCartItemAsync(updateCartDto.Id);

            if (item == null || item.CartId != cart.Id)
                throw new KeyNotFoundException("Cart item not found");

            if (updateCartDto.Quantity <= 0)
            {
                await _cartRepository.RemoveCartItemsAsync(item);
            }
            else
            {
                if ((item.ProductVariantId.HasValue && item.ProductVariant!.QuantityInStock < updateCartDto.Quantity) || (item.Product.QuantityInStock < updateCartDto.Quantity))
                {
                    throw new InvalidOperationException("Not enough stock available");
                }

                item.Quantity = updateCartDto.Quantity;
                await _cartRepository.UpdateCartItemAsync(item);
            }

            cart = await _cartRepository.GetCartByIdAsync(cart.Id);
            return _mapper.Map<CartDto>(cart);
        }
        public async Task<CartDto> RemoveCartItemAsync(string userId, string sessionId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);
            var item = await _cartRepository.GetCartItemAsync(cartItemId);

            if (item is null || item.CartId != cart.Id)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            await _cartRepository.RemoveCartItemsAsync(item);

            cart = await _cartRepository.GetCartByIdAsync(cart.Id);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> ClearCartAsync(string userId, string sessionId)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);
            await _cartRepository.ClearCartAsync(cart.Id);

            cart = await _cartRepository.GetByIdAsync(cart.Id);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync(string userId, string sessionId)
        {
            var cart = await _cartRepository.GetCartAsync(userId, sessionId);
            return _mapper.Map<CartSummaryDto>(cart);
        }
    }
}