using eShop.Data.Entities.UserAggregate;

namespace eShop.Data.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        IQueryable<Review> GetByProductIdAsync(int productId);
        Task<Review?> GetByIdWithUserAsync(int id);
        Task<bool> ExistsAsync(int productId, string userId);
    }
}