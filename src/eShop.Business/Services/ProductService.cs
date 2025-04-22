using eShop.Business.Interfaces;
using eShop.Data.Entities.Products;
using eShop.Business.Extensions;
using eShop.Data.Interfaces;
using eShop.Shared.Common.Pagination;
using eShop.Shared.Parameters;
using Microsoft.Extensions.Logging;

namespace eShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _productRepo = repository;
            _logger = logger;
        }

        public async Task<PagedList<Product>> GetProductsAsync(ProductParameters productParams)
        {
            try
            {
                return await _productRepo.GetAll()
                    .Search(productParams.SearchTerm)
                    .Sort(productParams.SortBy)
                    .Filter(productParams)
                    .ToPagedList(productParams.PageNumber, productParams.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all products");
                throw;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product is null)
                {
                    _logger.LogWarning($"Product with id {id} not found");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting product with id {id}");
                throw;
            }
        }

        public async Task<Product?> GetProductByUuidAsync(string uuid)
        {
            try
            {
                var product = await _productRepo.GetByUuidAsync(uuid);
                if (product is null)
                {
                    _logger.LogWarning($"Product with uuid {uuid} not found");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting product with uuid {uuid}");
                throw;
            }
        }

        public async Task CreateProductAsync(Product newProduct)
        {
            try
            {
                await _productRepo.AddAsync(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(updatedProduct.Id);

                if (product is null)
                {
                    throw new KeyNotFoundException($"Product with ID {updatedProduct.Id} not found");
                }

                await _productRepo.UpdateAsync(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with id {updatedProduct.Id}");
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _productRepo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while checking stock for product with id {id}");
                throw;
            }
        }
    }
}