using System.Runtime.CompilerServices;
using AutoMapper;
using eShop.Business.Interfaces;
using eShop.Data.Entities.UserAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Dashboard;
using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Reviews;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public DashboardService
        (
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepository,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            DashboardSummaryDto summary = new()
            {
                TotalProducts = await _productRepository.GetCountAsync(),
                TotalCategories = await _categoryRepository.GetCountAsync(),
                TotalUsers = await _userManager.Users.CountAsync(),
                AverageProductPrice = await _productRepository.GetAll().AverageAsync(p => p.BasePrice),
                AverageRating = await _reviewRepository.GetAll().AverageAsync(r => r.Rating),
                LowStockProductsCount = await _productRepository.GetAll().Where(p => p.QuantityInStock < 10).CountAsync(),
                TotalInventoryValue = await _productRepository.GetAll().SumAsync(p => p.QuantityInStock ?? 0),
            };

            return summary;
        }

        public async Task<IEnumerable<CategoryProductCountDto>> GetProductsByCategoryDistributionAsync(int? limit = null)
        {
            var totalCount = await _productRepository.GetCountAsync();
            var categoryProductCounts = await _productRepository.GetAll()
                .Include(p => p.Category)
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    CategoryName = g.Key.Name,
                    ProductCount = g.Count(),
                }).ToListAsync();

            var result = categoryProductCounts.Select(c => new CategoryProductCountDto
            {
                CategoryName = c.CategoryName,
                ProductCount = c.ProductCount,
                PercentageOfTotal = totalCount > 0 ? (decimal)c.ProductCount / totalCount : 0m,
            });

            if (limit.HasValue)
            {
                result = result.Take(limit.Value);
            }

            return result;
        }

        public async Task<IEnumerable<RatingDistributionDto>> GetRatingDistributionAsync()
        {
            return await _reviewRepository.GetAll()
                .GroupBy(r => r.Rating)
                .Select(g => new RatingDistributionDto()
                {
                    Count = g.Count(),
                    Rating = g.Key,
                }).ToListAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int? count = 5)
        {
            var recentReviews = _reviewRepository.GetAll()
                .OrderByDescending(r => r.CreatedDate)
                .AsQueryable();

            if (count.HasValue)
                recentReviews = recentReviews.Take(count.Value);

            return _mapper.Map<List<ReviewDto>>(await recentReviews.ToListAsync());
        }

        public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int? threshold = 10, int? count = 5)
        {
            var lowStockProducts = _productRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => (p.QuantityInStock ?? 0) < threshold);

            if (count.HasValue)
                lowStockProducts = lowStockProducts.Take(count.Value);

            return _mapper.Map<List<ProductDto>>(await lowStockProducts.ToListAsync());
        }

        public async Task<IEnumerable<ProductDto>> GetLatestFeaturedProductsAsync(int? count = 3)
        {
            var featuredProducts = _productRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.UpdatedDate.HasValue && p.UpdatedDate.Value > p.CreatedDate
                    ? p.UpdatedDate.Value
                    : p.CreatedDate)
                .AsQueryable();

            if (count.HasValue)
                featuredProducts = featuredProducts.Take(count.Value);

            return _mapper.Map<List<ProductDto>>(await featuredProducts.ToListAsync());
        }
    }
}