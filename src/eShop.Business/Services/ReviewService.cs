using AutoMapper;
using eShop.Business.Interfaces;
using eShop.Data.Entities.UserAggregate;
using eShop.Data.Interfaces;
using eShop.Shared.DTOs.Reviews;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(productId).ToListAsync();
            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> AddAsync(int productId, string? userId, CreateReviewDto createReviewDto)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("Unauthenticated user cannot add product review.");
            }

            if (await _reviewRepository.ExistsAsync(productId, userId))
            {
                throw new InvalidOperationException("User has already reviewed this product.");
            }

            var review = _mapper.Map<Review>(createReviewDto);
            review.UserId = userId;
            review.ProductId = productId;

            await _reviewRepository.AddAsync(review);
            return _mapper.Map<ReviewDto>(review);
        }
    }
}