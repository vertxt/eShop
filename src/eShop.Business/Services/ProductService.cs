using eShop.Business.Interfaces;
using eShop.Data.Entities.ProductAggregate;
using eShop.Business.Extensions;
using eShop.Data.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.Parameters;
using eShop.Shared.DTOs.Products;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> GetAllAsync(ProductParameters productParams)
        {
            var pagedResult = await _productRepository.GetAll()
                .Search(productParams.SearchTerm)
                .Sort(productParams.SortBy)
                .Filter(productParams)
                .ToPagedList(productParams.PageNumber, productParams.PageSize);

            return pagedResult.MapItems<Product, ProductDto>(_mapper);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> GetByUuidAsync(string uuid)
        {
            var product = await _productRepository.GetByUuidAsync(uuid);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with UUID {uuid} not found");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDetailDto> GetDetailByIdAsync(int id)
        {
            var product = await _productRepository.GetProductWithDetailsByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            return _mapper.Map<ProductDetailDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            await _productRepository.AddAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            _mapper.Map(updateProductDto, existingProduct);
            await _productRepository.UpdateAsync(existingProduct);

            return _mapper.Map<ProductDto>(existingProduct);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            await _productRepository.DeleteAsync(id);
        }


        // Variant methods
        public async Task<ProductVariantDto> GetVariantByIdAsync(int variantId)
        {
            var variant = await _productRepository.GetVariantByIdAsync(variantId);
            if (variant is null)
            {
                throw new KeyNotFoundException($"Variant with ID {variantId} not found");
            }

            return _mapper.Map<ProductVariantDto>(variant);
        }
        public async Task<int> AddVariantAsync(int productId, CreateProductVariantDto createProductVariantDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            var variant = _mapper.Map<ProductVariant>(createProductVariantDto);
            variant.ProductId = productId;

            if (!product.HasVariants)
            {
                product.HasVariants = true;
                await _productRepository.UpdateAsync(product);
            }

            await _productRepository.AddVariantAsync(variant);
            return variant.Id;
        }
        public async Task<bool> UpdateVariantAsync(int variantId, UpdateProductVariantDto updateProductVariantDto)
        {
            var variant = await _productRepository.GetVariantByIdAsync(variantId);
            if (variant is null)
            {
                throw new KeyNotFoundException($"Variant with ID {variantId} not found");
            }

            _mapper.Map(updateProductVariantDto, variant);
            return await _productRepository.UpdateVariantAsync(variant);
        }
        public async Task<bool> DeleteVariantAsync(int variantId)
        {
            var variant = await _productRepository.GetVariantByIdAsync(variantId);
            if (variant is null)
            {
                throw new KeyNotFoundException($"Variant with ID {variantId} not found");
            }

            var product = await _productRepository.GetByIdAsync(variant.ProductId);
            var result = await _productRepository.DeleteVariantAsync(variantId);

            if (result && product != null && product.Variants.Count <= 1)
            {
                product.HasVariants = false;
                await _productRepository.UpdateAsync(product);
            }

            return result;
        }

        // Image methods
        public async Task<int> AddImageAsync(CreateProductImageDto createProductImageDto, int? productId = null, int? variantId = null)
        {
            if (productId is null && variantId is null)
            {
                throw new ArgumentException("Either productId or variantId must be provided!");
            }

            var image = _mapper.Map<ProductImage>(createProductImageDto);
            image.ProductId = productId;
            image.ProductVariantId = variantId;

            IEnumerable<ProductImage> existingImages;

            if (productId.HasValue)
            {
                var product = await _productRepository.GetByIdAsync(productId.Value);
                if (product is null)
                {
                    throw new KeyNotFoundException($"Product with ID {productId} not found");
                }

                existingImages = await _productRepository.GetImagesByProductId(productId.Value).ToListAsync();
            }
            else
            {
                var variant = await _productRepository.GetVariantByIdAsync(variantId!.Value);
                if (variant is null)
                {
                    throw new KeyNotFoundException($"Variant with ID {variantId} not found");
                }

                existingImages = await _productRepository.GetImagesByVariantId(variantId.Value).ToListAsync();
            }

            if (!existingImages.Any() || createProductImageDto.IsMain)
            {
                image.IsMain = true;

                if (existingImages.Any() && createProductImageDto.IsMain)
                {
                    foreach (var existingImage in existingImages.Where(i => i.IsMain))
                    {
                        existingImage.IsMain = false;
                        await _productRepository.UpdateImageAsync(existingImage);
                    }
                }
            }

            await _productRepository.AddImageAsync(image);
            return image.Id;
        }

        public async Task<int> AddProductImageAsync(int productId, CreateProductImageDto createProductImageDto)
        {
            return await AddImageAsync(createProductImageDto, productId: productId);
        }
        public async Task<int> AddVariantImageAsync(int variantId, CreateProductImageDto createProductImageDto)
        {
            return await AddImageAsync(createProductImageDto, variantId: variantId);
        }

        public async Task<bool> UpdateProductImageAsync(int imageId, UpdateProductImageDto updateProductImageDto)
        {
            var image = await _productRepository.GetImageByIdAsync(imageId);
            if (image == null)
            {
                throw new KeyNotFoundException($"Image with ID {imageId} not found");
            }

            _mapper.Map(updateProductImageDto, image);

            if (updateProductImageDto.IsMain && !image.IsMain)
            {
                if (!image.ProductId.HasValue && !image.ProductVariantId.HasValue)
                {
                    throw new InvalidOperationException("Image is not associated with a product or variant.");
                }

                bool mainImageSetSuccess = image.ProductId.HasValue
                    ? await _productRepository.SetMainImageAsync(image.ProductId.Value, imageId)
                    : await _productRepository.SetMainVariantImageAsync(image.ProductVariantId!.Value, imageId);

                if (!mainImageSetSuccess)
                {
                    return false;
                }
            }

            return await _productRepository.UpdateImageAsync(image);
        }

        public async Task<bool> DeleteProductImageAsync(int imageId)
        {
            var image = await _productRepository.GetImageByIdAsync(imageId);
            if (image == null)
            {
                throw new KeyNotFoundException($"Image with ID {imageId} not found");
            }

            // If this is a main image, set another image as main
            if (image.IsMain)
            {
                IEnumerable<ProductImage> relatedImages;

                if (image.ProductId.HasValue)
                {
                    relatedImages = await _productRepository.GetImagesByProductId(image.ProductId.Value).ToListAsync();
                }
                else if (image.ProductVariantId.HasValue)
                {
                    relatedImages = await _productRepository.GetImagesByVariantId(image.ProductVariantId.Value).ToListAsync();
                }
                else
                {
                    throw new InvalidOperationException("Image is not associated with a product or variant.");
                }

                // Set the first remaining image as main (if any exist)
                var firstImage = relatedImages.FirstOrDefault(i => i.Id != imageId);
                if (firstImage != null)
                {
                    firstImage.IsMain = true;
                    await _productRepository.UpdateImageAsync(firstImage);
                }
            }

            return await _productRepository.DeleteImageAsync(imageId);
        }

        // Attribute methods
    }
}