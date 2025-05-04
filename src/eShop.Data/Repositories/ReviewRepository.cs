using eShop.Data.Entities.UserAggregate;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Review> GetByProductIdAsync(int productId)
        {
            return _entities.Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedDate);
        }

        public async Task<Review?> GetByIdWithUserAsync(int id)
        {
            return await _entities.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExistsAsync(int productId, string userId)
        {
            return await _entities.AnyAsync(r => r.ProductId == productId && r.UserId == userId);
        }
    }
}