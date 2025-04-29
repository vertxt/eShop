using eShop.Business.Interfaces;
using eShop.Data.Entities.ProductAggregate;
using eShop.Business.Extensions;
using eShop.Data.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.Parameters;
using eShop.Shared.DTOs.Products;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace eShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IImageService imageService, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        private string GenerateSku(string productName, string variantName = "base")
        {
            string baseName = $"{productName}-{variantName}".ToUpper().Replace(" ", "-");
            string suffix = DateTime.UtcNow.Ticks.ToString("N")[..6];
            return $"{baseName}-{suffix}";
        }

        public async Task<PagedList<ProductDto>> GetAllAsync(ProductParameters productParams)
        {
            var pagedResult = await _productRepository.GetAllWithBasicDetails()
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
            var product = await _productRepository.GetByIdWithDetailsAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            return _mapper.Map<ProductDetailDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found");
            }

            if (dto.Attributes.Any())
            {
                foreach (var attributeDto in dto.Attributes)
                {
                    var categoryAttribute = await _categoryRepository.GetAttributeByIdAsync(attributeDto.AttributeId);
                    if (categoryAttribute is null || categoryAttribute.CategoryId != dto.CategoryId)
                    {
                        throw new InvalidOperationException($"Invalid attribute ID {attributeDto.AttributeId} for category {dto.CategoryId}");
                    }
                }
            }

            var product = _mapper.Map<Product>(dto);
            product.HasVariants = dto.Variants?.Any() == true;

            if (dto.Images.Count != dto.ImageMetadata.Count)
            {
                throw new InvalidOperationException($"Not every image associated with a metadata. ({dto.Images.Count} and {dto.ImageMetadata.Count})");
            }

            for (int i = 0; i < dto.Images.Count; i++)
            {
                var image = dto.Images[i];
                var metadata = dto.ImageMetadata[i];

                var uploadResult = await _imageService.AddImageAsync(image);

                if (uploadResult.Error is not null)
                {
                    throw new Exception("Failed to upload image to cloud storage");
                }

                product.Images.Add(new ProductImage
                {
                    Url = uploadResult.SecureUrl.AbsoluteUri,
                    PublicId = uploadResult.PublicId,
                    DisplayOrder = metadata.DisplayOrder,
                    IsMain = metadata.IsMain,
                });
            }

            if (!product.Images.Any(i => i.IsMain))
            {
                product.Images.First().IsMain = true;
            }

            var variants = dto.Variants?.Select(v =>
            {
                var variant = _mapper.Map<ProductVariant>(v);
                if (string.IsNullOrEmpty(variant.SKU))
                {
                    variant.SKU = GenerateSku(product.Name, variant.Name);
                }
                return variant;
            }).ToList();
            if (variants is not null)
            {
                product.Variants = variants;
            }

            var attributes = dto.Attributes?.Select(a => _mapper.Map<ProductAttribute>(a)).ToList();
            if (attributes is not null)
            {
                product.Attributes = attributes;
            }

            await _productRepository.CreateProductWithRelationsAsync(product);

            var saved = await _productRepository.GetByIdWithDetailsAsync(product.Id);
            return _mapper.Map<ProductDto>(saved);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            // Update scalar fields
            _mapper.Map(dto, product);

            // Update attributes
            product.Attributes.Clear();
            foreach (var a in dto.Attributes)
            {
                product.Attributes.Add(new ProductAttribute
                {
                    Value = a.Value,
                    AttributeId = a.AttributeId,
                });
            }

            // Update variants
            var incomingIds = dto.Variants
                .Where(v => v.Id.HasValue)
                .Select(v => v.Id!.Value)
                .ToList();
            foreach (var existing in product.Variants.ToList())
            {
                if (!incomingIds.Contains(existing.Id))
                    product.Variants.Remove(existing);
            }

            foreach (var v in dto.Variants)
            {
                if (v.Id.HasValue)
                {
                    var existing = product.Variants
                        .Single(x => x.Id == v.Id.Value);
                    _mapper.Map(v, existing);
                }
                else
                {
                    product.Variants.Add(_mapper.Map<ProductVariant>(v));
                }
            }

            // Update images
            foreach (var image in product.Images.Where(i => !dto.ExistingImageIds.Contains(i.Id)).ToList())
            {
                if (image.PublicId is not null)
                {
                    var deletionResult = await _imageService.DeleteImageAsync(image.PublicId);
                    if (deletionResult.Error is not null)
                    {
                        throw new InvalidOperationException("Failed to remove image from cloud storage.");
                    }
                }
                product.Images.Remove(image);
            }

            for (int i = 0; i < dto.ExistingImageIds.Count; i++)
            {
                var imageId = dto.ExistingImageIds[i];
                var metadata = dto.ExistingImageMetadata[i];

                var image = product.Images
                    .Single(x => x.Id == imageId);
                image.IsMain = metadata.IsMain;
                image.DisplayOrder = metadata.DisplayOrder;
            }

            for (int i = 0; i < dto.Images.Count; i++)
            {
                var image = dto.Images[i];
                var metadata = dto.ImageMetadata[i];

                var uploadResult = await _imageService.AddImageAsync(image);

                if (uploadResult.Error is not null)
                {
                    throw new Exception("Failed to upload image to cloud storage");
                }

                product.Images.Add(new ProductImage
                {
                    Url = uploadResult.SecureUrl.AbsoluteUri,
                    PublicId = uploadResult.PublicId,
                    IsMain = metadata.IsMain,
                    DisplayOrder = metadata.DisplayOrder,
                });
            }

            await _productRepository.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            foreach (var image in product.Images)
            {
                if (image.PublicId is not null)
                {
                    var deletionResult = await _imageService.DeleteImageAsync(image.PublicId);
                    if (deletionResult.Error is not null)
                    {
                        throw new InvalidOperationException("Failed to remove image from cloud storage.");
                    }
                }
            }

            await _productRepository.DeleteAsync(id);
        }


        /* UNUSED METHODS (commented out) */
        // Variant methods
        // public async Task<ProductVariantDto> GetVariantByIdAsync(int variantId)
        // {
        //     var variant = await _productRepository.GetVariantByIdAsync(variantId);
        //     if (variant is null)
        //     {
        //         throw new KeyNotFoundException($"Variant with ID {variantId} not found");
        //     }

        //     return _mapper.Map<ProductVariantDto>(variant);
        // }
        // public async Task<int> AddVariantAsync(int productId, CreateProductVariantDto createProductVariantDto)
        // {
        //     var product = await _productRepository.GetByIdAsync(productId);
        //     if (product is null)
        //     {
        //         throw new KeyNotFoundException($"Product with ID {productId} not found");
        //     }

        //     var variant = _mapper.Map<ProductVariant>(createProductVariantDto);
        //     variant.ProductId = productId;

        //     if (string.IsNullOrEmpty(variant.SKU))
        //     {
        //         variant.SKU = GenerateSku(product.Name, variant.Name);
        //     }

        //     if (!product.HasVariants)
        //     {
        //         product.HasVariants = true;
        //         await _productRepository.UpdateAsync(product);
        //     }

        //     await _productRepository.AddVariantAsync(variant);
        //     return variant.Id;
        // }
        // public async Task<bool> UpdateVariantAsync(int variantId, UpdateProductVariantDto updateProductVariantDto)
        // {
        //     var variant = await _productRepository.GetVariantByIdAsync(variantId);
        //     if (variant is null)
        //     {
        //         throw new KeyNotFoundException($"Variant with ID {variantId} not found");
        //     }

        //     _mapper.Map(updateProductVariantDto, variant);
        //     return await _productRepository.UpdateVariantAsync(variant);
        // }
        // public async Task<bool> DeleteVariantAsync(int variantId)
        // {
        //     var variant = await _productRepository.GetVariantByIdAsync(variantId);
        //     if (variant is null)
        //     {
        //         throw new KeyNotFoundException($"Variant with ID {variantId} not found");
        //     }

        //     var product = await _productRepository.GetByIdAsync(variant.ProductId);
        //     var result = await _productRepository.DeleteVariantAsync(variantId);

        //     if (result && product != null && product.Variants.Count <= 1)
        //     {
        //         product.HasVariants = false;
        //         await _productRepository.UpdateAsync(product);
        //     }

        //     return result;
        // }

        // Image methods
        // public async Task<int> AddImageAsync(CreateProductImageDto createProductImageDto, int? productId = null, int? variantId = null)
        // {
        //     if (productId is null && variantId is null)
        //     {
        //         throw new ArgumentException("Either productId or variantId must be provided!");
        //     }

        //     var image = _mapper.Map<ProductImage>(createProductImageDto);
        //     image.ProductId = productId;
        //     image.ProductVariantId = variantId;

        //     IEnumerable<ProductImage> existingImages;

        //     if (productId.HasValue)
        //     {
        //         var product = await _productRepository.GetByIdAsync(productId.Value);
        //         if (product is null)
        //         {
        //             throw new KeyNotFoundException($"Product with ID {productId} not found");
        //         }

        //         existingImages = await _productRepository.GetImagesByProductId(productId.Value).ToListAsync();
        //     }
        //     else
        //     {
        //         var variant = await _productRepository.GetVariantByIdAsync(variantId!.Value);
        //         if (variant is null)
        //         {
        //             throw new KeyNotFoundException($"Variant with ID {variantId} not found");
        //         }

        //         existingImages = await _productRepository.GetImagesByVariantId(variantId.Value).ToListAsync();
        //     }

        //     if (!existingImages.Any() || createProductImageDto.IsMain)
        //     {
        //         image.IsMain = true;

        //         if (existingImages.Any() && createProductImageDto.IsMain)
        //         {
        //             foreach (var existingImage in existingImages.Where(i => i.IsMain))
        //             {
        //                 existingImage.IsMain = false;
        //                 await _productRepository.UpdateImageAsync(existingImage);
        //             }
        //         }
        //     }

        //     await _productRepository.AddImageAsync(image);
        //     return image.Id;
        // }

        // public async Task<int> AddProductImageAsync(int productId, CreateProductImageDto createProductImageDto)
        // {
        //     return await AddImageAsync(createProductImageDto, productId: productId);
        // }
        // public async Task<int> AddVariantImageAsync(int variantId, CreateProductImageDto createProductImageDto)
        // {
        //     return await AddImageAsync(createProductImageDto, variantId: variantId);
        // }

        // public async Task<bool> UpdateProductImageAsync(int imageId, UpdateProductImageDto updateProductImageDto)
        // {
        //     var image = await _productRepository.GetImageByIdAsync(imageId);
        //     if (image == null)
        //     {
        //         throw new KeyNotFoundException($"Image with ID {imageId} not found");
        //     }

        //     _mapper.Map(updateProductImageDto, image);

        //     if (updateProductImageDto.IsMain && !image.IsMain)
        //     {
        //         if (!image.ProductId.HasValue && !image.ProductVariantId.HasValue)
        //         {
        //             throw new InvalidOperationException("Image is not associated with a product or variant.");
        //         }

        //         bool mainImageSetSuccess = image.ProductId.HasValue
        //             ? await _productRepository.SetMainImageAsync(image.ProductId.Value, imageId)
        //             : await _productRepository.SetMainVariantImageAsync(image.ProductVariantId!.Value, imageId);

        //         if (!mainImageSetSuccess)
        //         {
        //             return false;
        //         }
        //     }

        //     return await _productRepository.UpdateImageAsync(image);
        // }

        // public async Task<bool> DeleteProductImageAsync(int imageId)
        // {
        //     var image = await _productRepository.GetImageByIdAsync(imageId);
        //     if (image == null)
        //     {
        //         throw new KeyNotFoundException($"Image with ID {imageId} not found");
        //     }

        //     // If this is a main image, set another image as main
        //     if (image.IsMain)
        //     {
        //         IEnumerable<ProductImage> relatedImages;

        //         if (image.ProductId.HasValue)
        //         {
        //             relatedImages = await _productRepository.GetImagesByProductId(image.ProductId.Value).ToListAsync();
        //         }
        //         else if (image.ProductVariantId.HasValue)
        //         {
        //             relatedImages = await _productRepository.GetImagesByVariantId(image.ProductVariantId.Value).ToListAsync();
        //         }
        //         else
        //         {
        //             throw new InvalidOperationException("Image is not associated with a product or variant.");
        //         }

        //         // Set the first remaining image as main (if any exist)
        //         var firstImage = relatedImages.FirstOrDefault(i => i.Id != imageId);
        //         if (firstImage != null)
        //         {
        //             firstImage.IsMain = true;
        //             await _productRepository.UpdateImageAsync(firstImage);
        //         }
        //     }

        //     return await _productRepository.DeleteImageAsync(imageId);
        // }

        // Attribute methods
        // public async Task<ProductAttributeDto> GetAttributeByIdAsync(int attributeId)
        // {
        //     var attribute = await _productRepository.GetAttributeByIdAsync(attributeId);
        //     if (attribute is null)
        //     {
        //         throw new KeyNotFoundException($"Product attribute with ID {attributeId} not found");
        //     }

        //     return _mapper.Map<ProductAttributeDto>(attribute);
        // }

        // public async Task<int> AddAttributeAsync(int productId, CreateProductAttributeDto createProductAttributeDto)
        // {
        //     var product = await _productRepository.GetByIdAsync(productId);
        //     if (product is null)
        //     {
        //         throw new KeyNotFoundException($"Product with ID {productId} not found");
        //     }

        //     var categoryAttribute = await _categoryRepository.GetAttributeByIdAsync(createProductAttributeDto.AttributeId);
        //     if (categoryAttribute is null)
        //     {
        //         throw new InvalidOperationException($"Category attribute with ID {createProductAttributeDto.AttributeId} not found");
        //     }

        //     if (categoryAttribute.CategoryId != product.CategoryId)
        //     {
        //         throw new InvalidOperationException($"Category attribute with ID {createProductAttributeDto.AttributeId} does not belong to the product's category");
        //     }

        //     var attribute = _mapper.Map<ProductAttribute>(createProductAttributeDto);
        //     attribute.ProductId = productId;
        //     await _productRepository.AddAttributeAsync(attribute);

        //     return attribute.Id;
        // }

        // public async Task<bool> UpdateAttributeAsync(int attributeId, UpdateProductAttributeDto updateProductAttributeDto)
        // {
        //     var attribute = await _productRepository.GetAttributeByIdAsync(attributeId);
        //     if (attribute is null)
        //     {
        //         throw new KeyNotFoundException($"Product attribute with ID {attributeId} not found");
        //     }

        //     _mapper.Map(updateProductAttributeDto, attribute);
        //     return await _productRepository.UpdateAttributeAsync(attribute);
        // }

        // public async Task<bool> DeleteAttributeAsync(int attributeId)
        // {
        //     var attribute = await _productRepository.GetAttributeByIdAsync(attributeId);
        //     if (attribute is null)
        //     {
        //         throw new KeyNotFoundException($"Product attribute with ID {attributeId} not found");
        //     }

        //     return await _productRepository.DeleteAttributeAsync(attributeId);
        // }

        // public async Task<IEnumerable<ProductAttributeDto>> GetAttributesByProductIdAsync(int productId)
        // {
        //     var product = await _productRepository.GetByIdAsync(productId);
        //     if (product is null)
        //     {
        //         throw new KeyNotFoundException($"Product with ID {productId} not found");
        //     }

        //     var attributes = product.Attributes;
        //     return _mapper.Map<IEnumerable<ProductAttributeDto>>(attributes);
        // }
    }
}
