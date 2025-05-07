using eShop.Business.Interfaces;
using eShop.Data.Entities.ProductAggregate;
using eShop.Business.Extensions;
using eShop.Data.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.Parameters;
using eShop.Shared.DTOs.Products;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using AutoMapper.QueryableExtensions;

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
            var query = _productRepository.GetAll()
                .Search(productParams.SearchTerm)
                .Sort(productParams.SortBy)
                .Filter(productParams);

            var pagedResult = await query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToPagedList(productParams.PageNumber, productParams.PageSize);

            return pagedResult;
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

            if (dto.Attributes.Count != 0)
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
            product.HasVariants = (dto.Variants?.Count ?? 0) > 0;

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

            if (!product.Images.Any(i => i.IsMain) && product.Images.Count > 0)
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

            await _productRepository.AddAsync(product);

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
            // Update HasVarients boolean prop
            product.HasVariants = product.Variants.Count > 0;

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
                _productRepository.RemoveImageAsync(image);
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
    }
}
