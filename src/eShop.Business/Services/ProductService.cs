using eShop.Business.Interfaces;
using eShop.Data.Entities.Products;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository repository)
        {
            _productRepo = repository;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepo.GetAllAsync().ToListAsync();
        }
        
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetByIdAsync(id);
        }
    }
}